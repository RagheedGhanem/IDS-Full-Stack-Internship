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
            // Retrieve all votes and return
            var allVotes = dbContext.Votes
                .Select(vote => new VoteDTO
                {
                    Type = vote.Type
                })
                .ToList();

            return Ok(allVotes);
        }

        // POST: api/Votes
        [HttpPost]
        public IActionResult CreateVote([FromBody] VoteDTO voteDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map VoteDTO to the Vote entity
            var newVote = new Vote
            {
                Type = voteDTO.Type
            };

            // Add the new vote to the database
            dbContext.Votes.Add(newVote);
            dbContext.SaveChanges();

            // Return the created vote's details
            return CreatedAtAction(nameof(GetAllVotes), new { id = newVote.Id }, newVote);
        }
    }
}
