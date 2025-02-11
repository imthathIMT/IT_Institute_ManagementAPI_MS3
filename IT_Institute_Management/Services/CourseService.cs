﻿using IT_Institute_Management.Database;
using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.EmailSection.Models;
using IT_Institute_Management.EmailSection.Service;
using IT_Institute_Management.Entity;
using IT_Institute_Management.ImageService;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;

namespace IT_Institute_Management.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IImageService _imageService;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly InstituteDbContext _context;
        private readonly sendmailService _sendmailService;

        public CourseService(ICourseRepository courseRepository, IStudentRepository studentRepository, IAnnouncementRepository announcementRepository, IImageService imageService, IHostEnvironment hostEnvironment, InstituteDbContext context, sendmailService sendmailService)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _announcementRepository = announcementRepository;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _context = context;
            _sendmailService = sendmailService;
        }

        public async Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllCoursesAsync();
            return courses.Select(course => new CourseResponseDTO
            {
                Id = course.Id,
                CourseName = course.CourseName,
                Level = course.Level,
                Duration = course.Duration,
                Fees = course.Fees,
                ImagePaths = course.ImagePaths.Split(',').ToList(),
                Description = course.Description,
            });
        }


        public async Task<CourseResponseDTO> GetCourseByIdAsync(Guid id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");

            return new CourseResponseDTO
            {
                Id = course.Id,
                CourseName = course.CourseName,
                Level = course.Level,
                Duration = course.Duration,
                Fees = course.Fees,
                ImagePaths = course.ImagePaths.Split(',').ToList(),
                Description = course.Description,
            };
        }


        public async Task CreateCourseAsync(CourseRequestDTO courseRequest, List<IFormFile> images)
        {

            if (courseRequest.Level != "Beginner" && courseRequest.Level != "Intermediate")
            {
                throw new Exception("Level must be either 'Beginner' or 'Intermediate'.");
            }


            if (courseRequest.Duration != 2 && courseRequest.Duration != 6)
            {
                throw new Exception("Duration must be either '2' or '6' months.");
            }

            var imagePaths = await SaveImagesAsync(images);

            var course = new Course
            {
                CourseName = courseRequest.CourseName,
                Level = courseRequest.Level,
                Duration = courseRequest.Duration,
                Fees = courseRequest.Fees,
                ImagePaths = string.Join(",", imagePaths),
                Description = courseRequest.Description,
            };

            await _courseRepository.AddCourseAsync(course);


            var announcement = new Announcement
            {
                Title = $"New Course offreing: {course.CourseName}",
                Body = $"We are offrerring new course: {course.CourseName}." +
                $" \n Level: {course.Level}, " +
                $"\n Duration: {course.Duration} " +
                $"\n months, Fees: {course.Fees}.",
                Date = DateTime.UtcNow
            };
            await _announcementRepository.AddAsync(announcement);

                var sendMailRequest = new SendMailRequest
            {
                CourseName = course.CourseName,
                Duration = course.Duration,
                Fees = course.Fees,
                Level = course.Level,
                TemplateName = "NewCourseOffering"

                };

            if (_sendmailService == null)
            {
                throw new InvalidOperationException("_sendmailService is not initialized.");
            }

            // Uncomment the email service once setup is correct
            // _emailService.SendRegistraionMail(studentDto.Email, studentDto);
            await _sendmailService.SendBulkCourseEmail(sendMailRequest);

            


        }


        private async Task<List<string>> SaveImagesAsync(List<IFormFile> images)
        {
            var imagePaths = new List<string>();

            foreach (var image in images)
            {

                var imagePath = await _imageService.SaveImage(image, "courses");
                imagePaths.Add(imagePath);
            }

            return imagePaths;
        }


        public async Task UpdateCourseAsync(Guid id, CourseRequestDTO courseRequest, List<IFormFile> images)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");

            var imagePaths = new List<string>();
            if (images != null && images.Any())
            {

                foreach (var image in images)
                {
                    var imagePath = await _imageService.SaveImage(image, "courses");
                    imagePaths.Add(imagePath);
                }
            }
            else
            {

                imagePaths = course.ImagePaths.Split(",").ToList();
            }

            if (courseRequest.Level != "Beginner" && courseRequest.Level != "Intermediate")
            {
                throw new Exception("Level must be either 'Beginner' or 'Intermediate'.");
            }


            if (courseRequest.Duration != 2 && courseRequest.Duration != 6)
            {
                throw new Exception("Duration must be either '2' or '6' months.");
            }


            course.CourseName = courseRequest.CourseName;
            course.Level = courseRequest.Level;
            course.Duration = courseRequest.Duration;
            course.Fees = courseRequest.Fees;
            course.ImagePaths = string.Join(",", imagePaths);
            course.Description = courseRequest.Description;

            await _courseRepository.UpdateCourseAsync(course);


            var announcement = new Announcement
            {
                Title = $"Updated Course: {course.CourseName}",
                Body = $"The course has been updated: {course.CourseName}. Level: {course.Level}, Duration: {course.Duration} months, Fees: {course.Fees}.",
                Date = DateTime.UtcNow
            };
            await _announcementRepository.AddAsync(announcement);

            var sendMailRequest = new SendMailRequest
            {
                CourseName = course.CourseName,
                Duration = course.Duration,
                Fees = course.Fees,
                Level = course.Level,
                TemplateName = "CourseUpdateNotification"

            };

            if (_sendmailService == null)
            {
                throw new InvalidOperationException("_sendmailService is not initialized.");
            }

            // Uncomment the email service once setup is correct
            // _emailService.SendRegistraionMail(studentDto.Email, studentDto);
            await _sendmailService.SendBulkCourseEmail(sendMailRequest);
        }

        public async Task DeleteCourseAsync(Guid id)
        {
            var courseExists = await _courseRepository.CourseExistsAsync(id);
            if (!courseExists)
                throw new KeyNotFoundException("Course not found.");

            var course = await _courseRepository.GetCourseByIdAsync(id);


            var imagePaths = course.ImagePaths.Split(',');
            foreach (var imagePath in imagePaths)
            {
                _imageService.DeleteImage(imagePath);
            }

            await _courseRepository.DeleteCourseAsync(id);


            var announcement = new Announcement
            {
                Title = "Course Deleted",
                Body = "A course has been deleted. Please check the course list for updates.",
                Date = DateTime.UtcNow
            };
            await _announcementRepository.AddAsync(announcement);

            

        }


    }
}
