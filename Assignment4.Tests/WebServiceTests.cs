﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using DAL;
using Xunit;

namespace Assignment4.Tests
{
    public class WebServiceTests
    {
        private const string CategoriesApi = "http://localhost:5001/api/categories";
        private const string ProductsApi = "http://localhost:5001/api/products";
        private const string PostsApi = "http://localhost:5001/api/posts";

        /* /api/categories */

        [Fact]
        public void ApiPosts_GetPostWithValidId_OkAndPostWithCommentsAndAnswers()
        {
            var (data, statusCode) = GetObject($"{PostsApi}/5821");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(2, data["answers"].ToImmutableList().Count);
            Assert.Equal(0, data["answers"][0]["comments"].ToImmutableList().Count);
            Assert.Equal(2, data["answers"][1]["comments"].ToImmutableList().Count);
            Assert.Equal(0, data["comments"].ToImmutableList().Count);
            Assert.Equal("SQL Server 2000/5 Escape an Underscore", data["title"]);
        }

        [Fact]
        public void ApiPosts_GetPostWithInvalidId_NotFound()
        {
            var (data, statusCode) = GetObject($"{PostsApi}/-1");

            Assert.Equal(HttpStatusCode.NotFound, statusCode);

        }

        [Fact]
        public void ApiPosts_SearchForPostWithTitle_statusOkAndListOfQuestions()
        {
            var (data, statusCode) = GetObject($"{PostsApi}/title/sql?pageSize=100");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(75, data["totalResults"].ToObject<int>());
            Assert.Equal("SQL Server 2000/5 Escape an Underscore", data["posts"][0]["title"]);
        }

        [Fact]
        public void ApiPosts_MarkPostValidId_Ok()
        {
            var (data, statusCode) = GetObject($"{PostsApi}/mark/5821");

            Assert.Equal(HttpStatusCode.OK, statusCode);

            DeleteData($"{PostsApi}/mark/5821");
        }

        [Fact]
        public void ApiPosts_MarkPostInvalidId_NotFound()
        {
            var (data, statusCode) = GetObject($"{PostsApi}/mark/-1");

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void ApiPosts_ClearHistory_Ok()
        {
            var frans = GetObject(PostsApi+"/title/sql?firstPage=true");

            var statusCode = DeleteData($"{PostsApi}/history");

            using (SovaContext db = new SovaContext())
            {
                Assert.Equal(0, db.History.Count());
            }

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void ApiPosts_GetHistory_OkAndListOfSearches()
        {
            DeleteData($"{PostsApi}/history");
            GetObject(PostsApi + "/title/sql?firstPage=true");
            GetObject(PostsApi + "/title/java?firstPage=true");

            var (data, statusCode) = GetArray($"{PostsApi}/history");

            Assert.Equal("sql", data[0]["text"]);
            Assert.Equal("java", data[1]["text"]);

        }

        [Fact]
        public void ApiPosts_CreateNoteValidId_OkAndNote()
        {
            var note = new
            {
                text = "Test"
            };

            var (data, statusCode) = PostData($"{PostsApi}/5821/note", note);

            Assert.Equal(data["text"], note.text);

            Assert.Equal(HttpStatusCode.OK, statusCode);

        }

        [Fact]
        public void ApiPosts_CreateNoteInvalidId_NotFound()
        {
            var note = new
            {
                text = "Test"
            };

            var (data, statusCode) = PostData($"{PostsApi}/-1/note", note);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);

        }


        [Fact]
        public void ApiPosts_UpdateNoteValidId_OkAndNote()
        {

            var newNote = new
            {
                text = "Test"
            };

            var (notes, status) = GetArray($"{PostsApi}/5821/note");
            var oldNote = notes[0];
            var statusCode = PutData($"{PostsApi}/5821/note/"+oldNote["id"], newNote);
            (notes, status) = GetArray($"{PostsApi}/5821/note");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(notes[0]["text"], newNote.text);
            newNote = new
            {
                text = oldNote["text"].ToString()
            }; 
            PutData($"{PostsApi}/5821/note/" + oldNote["id"], newNote);
        }

        [Fact]
        public void ApiPosts_GetNotesValidPostId_OkAndNotes()
        {
            var note = new
            {
                text = "Test"
            };

            var statusCode = PutData($"{PostsApi}/5821/note/5", note);

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        // Helpers

        (JArray, HttpStatusCode) GetArray(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JArray)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) GetObject(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) PostData(string url, object content)
        {
            var client = new HttpClient();
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        HttpStatusCode PutData(string url, object content)
        {
            var client = new HttpClient();
            var response = client.PutAsync(
                url,
                new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.UTF8,
                    "application/json")).Result;
            return response.StatusCode;
        }

        HttpStatusCode DeleteData(string url)
        {
            var client = new HttpClient();
            var response = client.DeleteAsync(url).Result;
            return response.StatusCode;
        }
    }
}

