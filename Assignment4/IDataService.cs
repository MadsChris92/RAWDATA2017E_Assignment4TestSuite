using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IDataService
    {

        List<Question> GetPostsByName(string name, int page, int pageSize, out int totalResults);
    
        List<Question> GetPostsByTagId(int tagId);

        Boolean MarkPost(int id);
        Boolean UnmarkPost(int id);
        Boolean AddHistory(string searchWord);
        Note GetNote(int postId);
        Note CreateNote(int postId, string text);
        Boolean DeleteNote(int noteId);
        Boolean ClearHistory();
        Question GetPost(int id);

        Question GetQuestionAllData(int id);
        User GetUser(int id);
    }
}
