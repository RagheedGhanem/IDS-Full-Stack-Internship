using IDS_App.Repository;
using IDS_App.DTOs;
using IDS_App.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IDS_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly IdsdatabaseDbContext dbContext;

        public VotesController(IdsdatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/Votes
        [HttpGet]
        public IActionResult GetAllVotes()
        {
            // we retrieve all votes
            var allVotes = dbContext.Votes
                .Select(vote => new VoteDTO
                {
                    Type = vote.Type,
                    PostId = vote.PostId,
                    UserId = vote.UserId
                })
                .ToList();

            return Ok(allVotes);
        }

        [HttpPost]
        public IActionResult CreateVote([FromBody] VoteDTO voteDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (voteDTO.Type?.ToLower() != "like" && voteDTO.Type?.ToLower() != "dislike")
            {
                return BadRequest("Vote type must be 'like' or 'dislike'.");
            }

            // Check if user already voted
            var existingVote = dbContext.Votes
                .FirstOrDefault(v => v.PostId == voteDTO.PostId && v.UserId == voteDTO.UserId);

            if (existingVote != null)
            {
                // Update existing vote
                existingVote.Type = voteDTO.Type;
                dbContext.SaveChanges();
                return Ok("Vote updated successfully.");
            }

            var newVote = new Vote
            {
                Type = voteDTO.Type,
                PostId = voteDTO.PostId,
                UserId = voteDTO.UserId
            };

            dbContext.Votes.Add(newVote);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetAllVotes), new { id = newVote.Id }, newVote);
        }

    }
}