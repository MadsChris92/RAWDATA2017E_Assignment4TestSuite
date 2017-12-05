using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Xunit;

namespace Assignment4.Tests
{
    public class DataServiceTests
    {
        private const int ValidPostId = 841339;
        private static DataService service = new DataService();

        /* Search */

        [Fact]
        public void GetQuestionByTitle_ArgumentWithResults_ReturnsListOfSearchQuestion()
        {
            var results = service.GetQuestionByTitle("sql", 0, 10, out var totalResults);
            Assert.Equal(84, totalResults);
            Assert.Equal(ValidPostId, results.First().Id);
        }

        [Fact]
        public void GetQuestionByTag_ArgumentWithResults_ReturnsListOfSearchQuestion()
        {
            var results = service.GetQuestionByTag("sql", 0, 10, out var totalResults);
            Assert.Equal(52, totalResults);
            Assert.Equal(60174, results.First().Id);
        }

        /* Marking */

        [Fact]
        public void MarkPost_ValidId_ReturnsTrue()
        {
            var result = service.MarkPost(ValidPostId);
            Assert.True(result);

            //Cleanup
            service.UnmarkPost(ValidPostId);
        }

        [Fact]
        public void MarkPost_InvalidId_ReturnsFalse()
        {
            var result = service.MarkPost(-1);
            Assert.False(result);
        }

        [Fact]
        public void UnmarkPost_ValidId_ReturnsTrue()
        {
            service.MarkPost(ValidPostId);

            var result = service.UnmarkPost(ValidPostId);
            Assert.True(result);
        }

        [Fact]
        public void UnmarkPost_InvalidId_ReturnsFalse()
        {
            var result = service.UnmarkPost(-1);
            Assert.False(result);
        }

        /* History */

        [Fact]
        public void AddHistory_ValidString_ReturnsTrue()
        {
            var result = service.AddHistory("test");
            Assert.True(result);
        }

        [Fact]
        public void AddHistory_Null_ReturnsFalse()
        {
            var result = service.AddHistory(null);
            Assert.False(result);
        }

        [Fact]
        public void GetHistory__ReturnsListOfHistory()
        {
            service.ClearHistory();
            service.AddHistory("item1");
            service.AddHistory("item2");

            var result = service.GetHistory(0, 10, out var totalResults);
            Assert.NotNull(result);
            Assert.Equal(2, totalResults);
            Assert.Contains(result, history => history.Text == "item1");

            //Clean
            service.ClearHistory();
        }

        /*
        [Fact]
        public void GetHistory_InvalidId_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.GetHistory(-1);
            Assert.False(result);
        }

        [Fact]
        public void GetHistory_ValidId_ReturnsTrue()
        {
            var service = new DataService();
            service.ClearHistory();
            service.AddHistory("item1");
            var id = service.GetHistory().First().Id;
            var result = service.GetHistory(id);
            Assert.True(result);
        }

        [Fact]
        public void DeleteHistory_InvalidId_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.DeleteHistory(-1);
            Assert.False(result);
        }

        [Fact]
        public void DeleteHistory_ValidId_ReturnsTrue()
        {
            var service = new DataService();
            service.ClearHistory();
            service.AddHistory("item1");
            var id = service.GetHistory().First().Id;
            var result = service.DeleteHistory(id);
            Assert.True(result);
        }
        */

        /* Notes */

        //Create
        [Fact]
        public void CreateNote_ValidPostId_Note()
        {
            var result = service.CreateNote(ValidPostId, "Make note of this");
            
            Assert.NotNull(result);

            var notes = service.GetNotes(ValidPostId, 0, 100, out var _);

            Assert.Contains(notes, note => note.Text == "Make note of this");

            //Clean
            notes.ForEach(note => service.DeleteNote(note.Id));

        }

        [Fact]
        public void CreateNote_InvalidPostId_Null()
        {
            var result = service.CreateNote(-1, "Make null of this");

            Assert.Null(result);
        }
        //Read
        [Fact]
        public void ReadNote_ValidPostId_Note()
        {
            var createdNote = service.CreateNote(ValidPostId, "Make note of this");

            var note = service.GetNote(ValidPostId, createdNote.Id);

            Assert.Equal(createdNote.Text, note.Text);
            //Assert.Contains(notes, note => note.Text == "Make note of this");

            //Clean
            //notes.ForEach(note => service.DeleteNote(note.Id));
            service.DeleteNote(createdNote.Id);

        }

        [Fact]
        public void ReadNote_InvalidPostId_Null()
        {
            var notes = service.GetNote(-1);

            Assert.Equal(null, notes);

        }
        //Update
        //Delete
    }

}
