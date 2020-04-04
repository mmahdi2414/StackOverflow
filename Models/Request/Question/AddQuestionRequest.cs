using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTProject.Models.Request.Question
{
    public class AddQuestionRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
