using IDS_App.Repository;
using IDS_App.DTOs;
using IDS_App.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IDS_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IdsdatabaseDbContext dbContext;

        public CategoriesController(IdsdatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/Categories
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            // Retrieve all categories and map them to CategoryDTO
            var allCategories = dbContext.Categories
                .Select(category => new CategoryDTO
                {
                    Name = category.Name,
                    Description = category.Description
                })
                .ToList();

            return Ok(allCategories);
        }

        // POST: api/Categories
        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map CategoryDTO to the Category entity
            var newCategory = new Category
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description
            };

            // Add the new category to the database
            dbContext.Categories.Add(newCategory);
            dbContext.SaveChanges();

            // Return the created category's details
            return CreatedAtAction(nameof(GetAllCategories), new { id = newCategory.Id }, newCategory);
        }
    }
}
