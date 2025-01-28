using IDS_App.Repository;
using IDS_App.DTOs;
using IDS_App.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IDS_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IdsdatabaseDbContext dbContext;

        public CommentsController(IdsdatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/Comments
        [HttpGet]
        public IActionResult GetAllComments()
        {
            // Retrieve all comments and map them to CommentDTO
            var allComments = dbContext.Comments
                .Select(comment => new CommentDTO
                {
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt
                })
                .ToList();

            return Ok(allComments);
        }

        [HttpPost]
        public IActionResult CreateComment([FromBody] CommentDTO commentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the PostId exists in the database
            var postExists = dbContext.Posts.Any(p => p.Id == commentDTO.PostId);
            if (!postExists)
            {
                return BadRequest($"The specified Post with ID {commentDTO.PostId} does not exist.");
            }

            // If the PostId is valid, create the new comment
            var newComment = new Comment
            {
                Content = commentDTO.Content,
                CreatedAt = commentDTO.CreatedAt ?? DateTime.UtcNow,
                PostId = commentDTO.PostId,
                UserId = commentDTO.UserId // Optional if provided in the DTO
            };

            // Add the new comment to the database
            dbContext.Comments.Add(newComment);
            dbContext.SaveChanges();

            // Return the created comment's details
            return CreatedAtAction(nameof(GetAllComments), new { id = newComment.Id }, newComment);
        }
    }
}
