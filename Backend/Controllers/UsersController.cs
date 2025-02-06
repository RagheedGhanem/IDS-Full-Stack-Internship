using IDS_App.Repository;
using IDS_App.DTOs;
using IDS_App.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace IDS_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IdsdatabaseDbContext dbContext;

        public UsersController(IdsdatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // POST: api/users
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = new User
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Password = userDto.Password,
                };

                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                return CreatedAtAction(nameof(GetUserByUsernameAndEmail), new { username = userDto.Username, email = userDto.Email }, userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while creating the user.",
                    Error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }


        // POST: api/users/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Email == loginDto.Email);
                if (user == null)
                {
                    return Unauthorized(new { Message = "User not found." });
                }

                // Check if the password matches 
                if (user.Password != loginDto.Password)
                {
                    return Unauthorized(new { Message = "Invalid credentials." });
                }

                return Ok(new { Message = "Login successful", User = new { user.Username, user.Email } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while logging in.",
                    Error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }






        // GET: api/users/{username}/{email}
        [HttpGet("{username}/{email}")]
        public IActionResult GetUserByUsernameAndEmail(string username, string email)
        {
            try
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Username == username && u.Email == email);

                if (user == null)
                {
                    return NotFound(new { Message = "User not found." });
                }

                var userDto = new UserDTO
                {
                    Username = user.Username,
                    Email = user.Email,
                    Password = user.Password,
              
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while retrieving the user.",
                    Error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        // GET: api/users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = dbContext.Users.ToList();

                if (users == null || !users.Any())
                {
                    return NotFound(new { Message = "No users found." });
                }

                var userDtos = users.Select(user => new UserDTO
                {
                    Username = user.Username,
                    Email = user.Email,
                    Password = user.Password,
                }).ToList();

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while retrieving all users.",
                    Error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}