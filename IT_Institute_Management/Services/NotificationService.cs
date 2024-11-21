﻿using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using IT_Institute_Management.Repositories;

namespace IT_Institute_Management.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IStudentRepository _studentRepository;

        public NotificationService(INotificationRepository notificationRepository, IStudentRepository studentRepository)
        {
            _notificationRepository = notificationRepository;
            _studentRepository = studentRepository;
        }


        public async Task<IEnumerable<NotificationResponseDTO>> GetAllNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetAllNotificationsAsync();
            return notifications.Select(n => new NotificationResponseDTO
            {
                Id = n.Id,
                Message = n.Message,
                Date = n.Date,
                StudentNIC = n.StudentNIC
            });
        }

        public async Task<NotificationResponseDTO> GetNotificationByIdAsync(Guid id)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(id);
            if (notification == null)
                throw new KeyNotFoundException("Notification not found.");

            return new NotificationResponseDTO
            {
                Id = notification.Id,
                Message = notification.Message,
                Date = notification.Date,
                StudentNIC = notification.StudentNIC
            };
        }


        public async Task CreateNotificationAsync(NotificationRequestDTO notificationRequest)
        {
            var notification = new Notification
            {
                Message = notificationRequest.Message,
                Date = notificationRequest.Date,
                StudentNIC = notificationRequest.StudentNIC
            };

            await _notificationRepository.AddNotificationAsync(notification);
        }

        public async Task UpdateNotificationAsync(Guid id, NotificationRequestDTO notificationRequest)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(id);
            if (notification == null)
                throw new KeyNotFoundException("Notification not found.");

            notification.Message = notificationRequest.Message;
            notification.Date = notificationRequest.Date;
            notification.StudentNIC = notificationRequest.StudentNIC;

            await _notificationRepository.UpdateNotificationAsync(notification);
        }


        public async Task DeleteNotificationAsync(Guid id)
        {
            var notificationExists = await _notificationRepository.NotificationExistsAsync(id);
            if (!notificationExists)
                throw new KeyNotFoundException("Notification not found.");

            await _notificationRepository.DeleteNotificationAsync(id);
        }

        // Send notification to student based on NIC (or some unique student identifier)
        public async Task SendNotificationAsync(string studentNIC, string message)
        {
            var student = await _studentRepository.GetByNicAsync(studentNIC);
            if (student == null)
                throw new KeyNotFoundException("Student not found.");

            var notification = new Notification
            {
                Message = message,
                Date = DateTime.UtcNow,
                StudentNIC = studentNIC
            };

            // Save the notification to the database
            await _notificationRepository.AddNotificationAsync(notification);

          
        }
    }
}
