using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MTProject.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MTProject.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MTProject.Models.Response.Comment;
using MTProject.Models.Response.Question;
using MTProject.Models.Request.Question;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MTProject.Controllers
{
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly DataContext context;
        public QuestionController(UserManager<User> _userManager, DataContext _context)
        {
            userManager = _userManager;
            context = _context;
        }
        [Route("Add")]

        [HttpPost]
        public QuestionResponse AddQuestion([FromBody]AddQuestionRequest request)
        {
            var userId = User.Claims
                .Where<Claim>(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(s => s.Value).First();

            if(userId == null)
            {
                return new QuestionResponse { Data = null, Message = "اطلاعات کاربری یافت نشد" };
            }

            var question = new Question
            {
                Title = request.Title,
                Description = request.Description,
                UserId = userId,
                CreateTime = DateTime.Now,
            };
            try
            {
                context.Questions.Add(question);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                return new QuestionResponse { Data = null, Message = "خطایی در ثبت سوال بوجد امد" };
            }
            
            return new QuestionResponse {Data = null,Message = "سوال با موفقیت ثبت شد" };
        }
        [HttpGet]
        [AllowAnonymous]
        public GetQuestionResponse GetQuestion([FromQuery]int QuestionId)
        {
            var comments = new List<CommentResponse>();
            var answers = new List<AnswerResponse>();
            var question = context.Questions
                .Include(i => i.User)
                .Include(i => i.Comments)
                .ThenInclude(ti => ti.User)
                .Include(i => i.Answers).ThenInclude(ti => ti.Comments).ThenInclude(tti => tti.User)
                .Include(i => i.Answers).ThenInclude(ti => ti.DisLikes)
                .Include(i => i.Answers).ThenInclude(ti => ti.Likes)
                .SingleOrDefault(s => s.Id == QuestionId);
            if(question == null)
            {
                return new GetQuestionResponse { };
            }
            if (question.Comments != null)
            {
                foreach (var cmt in question.Comments)
                {
                    comments.Add(new CommentResponse
                    {
                        CreateTime = cmt.CreateTime,
                        Description = cmt.Description,
                        Id = cmt.Id,
                        UserName = cmt.User.FirstName
                    });
                }
            }
            if (question.Answers != null)
            {
                foreach (var ans in question.Answers)
                {
                    var answer = new AnswerResponse
                    {
                        CreateTime = ans.CreateTime,
                        Description = ans.Description,
                        Id = ans.Id,
                        UserName = ans.User.FirstName,
                        Likes = ans.Likes.Count,
                        DisLike = ans.DisLikes.Count,
                    };
                    
                    var AnswersComments = new List<CommentResponse>();
                    if (ans.Comments != null)
                    {
                        foreach (var cmt in ans.Comments)
                        {
                            AnswersComments.Add(new CommentResponse
                            {
                                CreateTime = cmt.CreateTime,
                                Description = cmt.Description,
                                Id = cmt.Id,
                                UserName = cmt.User.FirstName
                            });
                        }
                        answer.Comments = AnswersComments;
                    }
                    answers.Add(answer);
                }
            }
            DateTime t = new DateTime();
            return new GetQuestionResponse {
                // Id = QuestionId,
                // UserName = "question.User.FirstName",
                // Title = "question.Title",
                // Description = "question.Description",
                // CreateTime = t,
                // Comments = comments,
                // Answers = answers
                Id = question.Id,
                UserName = question.User.FirstName,
                Title = question.Title,
                Description = question.Description,
                CreateTime = question.CreateTime,
                Comments = comments,
                Answers = answers
            };
        }
        [Route("List")]
        [HttpGet]
        [AllowAnonymous]
        public List<GetQuestionListResponse> GetQuestionList()
        {
            
            List<GetQuestionListResponse> q = new List<GetQuestionListResponse>();
            q.Add(new GetQuestionListResponse{Id = 1,Title = "testt1"});
            return q;
            var questions = context.Questions.Select(s => new GetQuestionListResponse
            {
                Id = s.Id,
                Title = s.Title
            }).ToList();

            return questions;
        }
        [HttpPost]
        [Route("AddComment")]
        public QuestionResponse AddComment([FromBody]AddCommentRequest request)
        {
            var userId = User.Claims
                .Where<Claim>(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(s => s.Value).First();

            if (userId == null)
            {
                return new QuestionResponse { Data = null, Message = "اطلاعات کاربری یافت نشد" };
            }
            var comment = new Comment
            {
                UserId = userId,
                QuestionId = request.QuestionId,
                Description = request.Description,
                CreateTime = DateTime.Now
            };
            context.Comments.Add(comment);
            context.SaveChanges();
            return new QuestionResponse { };
        }
    }
}
