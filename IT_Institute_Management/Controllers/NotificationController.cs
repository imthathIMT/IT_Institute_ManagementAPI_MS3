﻿using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                var notifications = await _notificationService.GetAllNotificationsAsync();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(Guid id)
        {
            try
            {
                var notification = await _notificationService.GetNotificationByIdAsync(id);
                return Ok(notification);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Notification not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification(NotificationRequestDTO notificationRequest)
        {
            try
            {
                await _notificationService.CreateNotificationAsync(notificationRequest);
                return CreatedAtAction(nameof(GetNotificationById), new { id = notificationRequest.StudentNIC }, notificationRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(Guid id, NotificationRequestDTO notificationRequest)
        {
            try
            {
                await _notificationService.UpdateNotificationAsync(id, notificationRequest);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Notification not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
