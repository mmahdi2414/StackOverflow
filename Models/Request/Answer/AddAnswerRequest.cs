using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTProject.Models.Request.Answer
{
    public class AddAnswerRequest
    {
        public int QuestionId { get; set; }
        public string Description { get; set; }
    }
}
