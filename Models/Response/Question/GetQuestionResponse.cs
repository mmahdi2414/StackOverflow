using MTProject.Models.Response.Comment;
using MTProject.Models.Response.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTProject.Models.Response
{
    public class GetQuestionResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public string UserName { get; set; }
        public List<CommentResponse> Comments { get; set; }
        public List<AnswerResponse> Answers { get; set; }

    }
}
