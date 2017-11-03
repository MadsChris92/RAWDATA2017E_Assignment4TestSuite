using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IDataService
    {

        List<SearchQuestion> GetPostsByName(string name, int page, int pageSize, out int totalResults);
        List<SearchQuestion> GetPostsByTagTitle(string name, int page, int pageSize, out int totalResults);
        Boolean MarkPost(int id);
        Boolean UnmarkPost(int id);
        Boolean AddHistory(string searchWord);
        Note GetNote(int postId);
        Note CreateNote(int postId, string text);
        Boolean UpdateNote(int noteId, string text);
        Boolean DeleteNote(int noteId);
        Boolean ClearHistory();
        List<History> GetHistory();
        Question GetPost(int id);
        Question GetQuestionAllData(int id);
        User GetUser(int id);
    }

    
}
