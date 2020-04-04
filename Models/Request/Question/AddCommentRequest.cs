using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTProject.Models.Request.Question
{
    public class AddCommentRequest
    {
        public int QuestionId { get; set; }
        public string Description { get; set; }

    }
}
