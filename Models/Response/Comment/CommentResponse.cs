using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTProject.Models.Response.Comment
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public string UserName { get; set; }

        public static implicit operator List<object>(CommentResponse v)
        {
            throw new NotImplementedException();
        }
    }
}
