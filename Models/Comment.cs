using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MTProject.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public User User { get; set; }
        public Question Question { get; set; }
    }
}
