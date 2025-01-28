using IDS_App.Repository;
using IDS_App.DTOs;
using IDS_App.Repository.Models;
using Microsoft.AspNetCore.Mvc;
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

        // GET: api/Posts
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            // Retrieve all posts and map them to PostDTO
            var allPosts = dbContext.Posts
                .Select(post => new PostDTO
                {
                    Title = post.Title,
                    Description = post.Description,
                    CreatedAt = post.CreatedAt,
                    Tags = post.Tags,
                    UserId = post.UserId // Include UserId in the response
                })
                .ToList();

            return Ok(allPosts);
        }

        // POST: api/Posts
        [HttpPost]
        public IActionResult CreatePost([FromBody] PostDTO postDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure the UserID exists in the Users table
            var userExists = dbContext.Users.Any(user => user.Id == postDTO.UserId);
            if (!userExists)
            {
                return BadRequest("The specified UserID does not exist.");
            }

            // Map PostDTO to the Post entity
            var newPost = new Post
            {
                Title = postDTO.Title,
                Description = postDTO.Description,
                CreatedAt = postDTO.CreatedAt ?? DateTime.Now,
                Tags = postDTO.Tags,
                UserId = postDTO.UserId // Include UserID
            };

            // Add the new post to the database
            dbContext.Posts.Add(newPost);
            dbContext.SaveChanges();

            // Return the created post's details
            return CreatedAtAction(nameof(GetAllPosts), new { id = newPost.Id }, newPost);
        }
    }
}