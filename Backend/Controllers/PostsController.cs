using IDS_App.Repository;
using IDS_App.DTOs;
using IDS_App.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IDS_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IdsdatabaseDbContext dbContext;

        public PostsController(IdsdatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Enable CORS (for frontend API calls)
        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            return Ok();
        }

        // GET: api/Posts
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            var allPosts = dbContext.Posts
                .OrderByDescending(post => post.CreatedAt) // show latest posts first
                .Select(post => new PostDTO
                {
                    //Id = post.Id, 
                    Title = post.Title,
                    Description = post.Description,
                    CreatedAt = post.CreatedAt,
                    Tags = post.Tags,
                    UserId = post.UserId
                })
                .ToList();

            return Ok(allPosts);
        }

        
        [HttpPost]
        public IActionResult CreatePost([FromBody] PostDTO postDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // ensure the UserID exists in the Users table
            var userExists = dbContext.Users.Any(user => user.Id == postDTO.UserId);
            if (!userExists)
            {
                return BadRequest("The specified UserID does not exist.");
            }

            var newPost = new Post
            {
                Title = postDTO.Title,
                Description = postDTO.Description,
                CreatedAt = postDTO.CreatedAt ?? DateTime.UtcNow,
                Tags = postDTO.Tags,
                UserId = postDTO.UserId
            };

            // add to database
            dbContext.Posts.Add(newPost);
            dbContext.SaveChanges();

            // return the newly created post as JSON
            return Ok(new
            {
                id = newPost.Id,
                title = newPost.Title,
                description = newPost.Description,
                createdAt = newPost.CreatedAt,
                tags = newPost.Tags,
                userId = newPost.UserId
            });
        }

    }
}
