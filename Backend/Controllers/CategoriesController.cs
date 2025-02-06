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

            var newCategory = new Category
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description
            };

            dbContext.Categories.Add(newCategory);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetAllCategories), new { id = newCategory.Id }, newCategory);
        }
    }
}
