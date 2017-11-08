using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL
{
    public interface IDataService
    {

        List<SearchQuestion> GetPostsByName(string name, int page, int pageSize, out int totalResults);
        List<SearchQuestion> GetPostsByTagTitle(string name, int page, int pageSize, out int totalResults);
        Boolean MarkPost(int id);
        Boolean UnmarkPost(int id);
        Boolean AddHistory(string searchWord);
        List<Note> GetNotes(int postId, int page, int pageSize, out int totalResults);
        Note CreateNote(int postId, string text);
        Boolean UpdateNote(int noteId, string text);
        Boolean DeleteNote(int noteId);
        Boolean ClearHistory();
        List<History> GetHistory(int page, int pageSize, out int totalResults);
        Question GetPost(int id);
        Question GetQuestionAllData(int id);
        User GetUser(int id);
    }

    
}
