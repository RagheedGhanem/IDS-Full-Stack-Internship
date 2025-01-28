using IDS_App.Repository;
using IDS_App.DTOs;
using IDS_App.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IDS_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IdsdatabaseDbContext dbContext;

        public NotificationsController(IdsdatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/Notifications
        [HttpGet]
        public IActionResult GetAllNotifications()
        {
            // Retrieve all notifications and map to NotificationDTO
            var allNotifications = dbContext.Notifications
                .Select(notification => new NotificationDTO
                {
                    Message = notification.Message
                })
                .ToList();

            return Ok(allNotifications);
        }

        // POST: api/Notifications
        [HttpPost]
        public IActionResult CreateNotification([FromBody] NotificationDTO notificationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map NotificationDTO to Notification entity
            var newNotification = new Notification
            {
                Message = notificationDTO.Message
            };

            // Add the new notification to the database
            dbContext.Notifications.Add(newNotification);
            dbContext.SaveChanges();

            // Return the created notification's details
            return CreatedAtAction(nameof(GetAllNotifications), new { id = newNotification.Id }, newNotification);
        }
    }
}
