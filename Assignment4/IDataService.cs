using System;
using System.Collections.Generic;
using DAL.DomainObjects;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL
{
    public interface IDataService
    {
        List<SearchQuestion> GetQuestionByTitle(string title, int page, int pageSize, out int totalResults);
        List<RankedSearchQuestion> GetRankedQuestionByTitle(string title, int page, int pageSize, out int totalResults);
        Question GetQuestionAllData(int id);
        Post GetPost(int id);
        User GetUser(int id);
        List<SearchQuestion> GetQuestionByTag(string tag, int page, int pageSize, out int totalResults);
        List<Question> GetPostsHighestScore(int page, int pageSize, out int totalResults);
        Boolean MarkPost(int id);
        List<MarkedPost> GetMarkedPosts(int page, int pageSize, out int totalResults);
        bool UnmarkPost(int id);
        bool AddHistory(string searchWord);
        Note GetNote(int noteId);
        List<Note> GetNotes(int postId, int page, int pageSize, out int totalResults);
        Note CreateNote(int postId, string text);
        Boolean DeleteNote(int noteId);
        Boolean UpdateNote(int noteId, string text);
        bool ClearHistory();
        List<History> GetHistory(int page, int pageSize, out int totalResults);
        List<RankedWord> GetWeightedWordList(string name, int page, int pageSize, out int totalResults);
        List<RankedSearchQuestion> SearchPostsOrderedByScore(string title, int page, int pageSize, out int totalResults);
        List<RankedSearchQuestion> SearchPostsOrderedByNewest(string title, int page, int pageSize, string orderBy, out int totalResults);
        List<RankedSearchQuestion> SearchPostsOrderedByRanking(string title, int page, int pageSize, out int totalResults);
        List<RankedSearchQuestion> SearchPosts(string terms, int page, int pageSize, string orderBy, out int totalResults);
    }

    
}
