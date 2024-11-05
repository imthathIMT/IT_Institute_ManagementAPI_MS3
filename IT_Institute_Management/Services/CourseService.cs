﻿using IT_Institute_Management.DTO.ResponseDTO;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;

namespace IT_Institute_Management.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
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
                ImagePath = course.ImagePath
            });
        }
    }
}
