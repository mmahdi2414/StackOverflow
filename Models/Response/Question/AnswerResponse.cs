using MTProject.Models.Response.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTProject.Models.Response.Question
{
    public class AnswerResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public string UserName { get; set; }
        public List<CommentResponse> Comments { get; set; }
        public int Likes { get; set; }
        public int DisLike { get; set; }
    }
}
