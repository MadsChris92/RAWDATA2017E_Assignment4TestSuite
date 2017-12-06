using System;
using System.Collections.Generic;
using DAL.DomainObjects;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL
{
    public interface IDataService
    {

        List<SearchQuestion> GetQuestionByTitle(string title, int page, int pageSize, out int totalResults);
        List<SearchQuestion> GetQuestionByTag(string tag, int page, int pageSize, out int totalResults);
        Boolean MarkPost(int id);
        Boolean UnmarkPost(int id);
        Boolean AddHistory(string searchWord);
        List<Note> GetNotes(int postId, int page, int pageSize, out int totalResults);
        Note CreateNote(int postId, string text);
        Boolean UpdateNote(int noteId, string text);
        Boolean DeleteNote(int noteId);
        Boolean ClearHistory();
        List<History> GetHistory(int page, int pageSize, out int totalResults);
        Question GetQuestionAllData(int id);
        Post GetPost(int id);
        User GetUser(int id);
        List<RankedSearchQuestion> GetRankedQuestionByTitle(string title, int page, int pageSize, out int totalResults);
        List<RankedWord> GetWeightedWordList(string name, int page, int pageSize, out int totalResults);
    }

    
}
