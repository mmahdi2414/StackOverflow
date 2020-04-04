using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MTProject.Models;
using MTProject.Models.Response.Answer;
using MTProject.Models.Request.Answer;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MTProject.Controllers
{
    [Route("api/[controller]")]
    public class AnswerController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly DataContext context;
        public AnswerController(UserManager<User> _userManager, DataContext _context)
        {
            userManager = _userManager;
            context = _context;
        }
        
        public AnswerResponse AddAnswer([FromBody] AddAnswerRequest request)
        {
            var userId = User.Claims
                .Where<Claim>(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(s => s.Value).First();

            if (userId == null)
            {
                return new AnswerResponse { Data = null, Message = "اطلاعات کاربری یافت نشد" };
            }
            var answer = new Answer
            {
                CreateTime = DateTime.Now,
                Description = request.Description,
                QuestionId = request.QuestionId,
                UserId = userId
            };
            try
            {
                context.Answers.Add(answer);
                context.SaveChanges();
            }
            catch(Exception e)
            {
                return new AnswerResponse { Data = null,Message = "در ثبت جواب مشکلی پیش آمد" };
            }
            return new AnswerResponse { Data = null, Message = "جواب با موفقیت ثبت شد" };
        }
    }
}
