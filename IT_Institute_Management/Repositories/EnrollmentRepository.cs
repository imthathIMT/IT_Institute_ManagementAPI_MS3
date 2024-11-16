using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace IT_Institute_Management.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly InstituteDbContext _context;

        public EnrollmentRepository(InstituteDbContext context)
        {
            _context = context;
        }

        public async Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment)
        {
            await _context.Enrollment.AddAsync(enrollment);
            await SaveChangesAsync();
            return enrollment;
        }


        public async Task<Enrollment> GetEnrollmentByIdAsync(Guid id)
        {
            return await _context.Enrollment
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Enrollment> GetEnrollmentByNICAsync(string nic)
        {
            return await _context.Enrollment
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.StudentNIC == nic);
        }

        public async Task<Enrollment> DeleteEnrollmentAsync(Guid id)
        {
            var enrollment = await GetEnrollmentByIdAsync(id);
            if (enrollment != null)
            {
                _context.Enrollment.Remove(enrollment);
                await SaveChangesAsync();
            }
            return enrollment;
        }

        public async Task<Enrollment> DeleteEnrollmentByNICAsync(string nic)
        {
            var enrollment = await GetEnrollmentByNICAsync(nic);
            if (enrollment != null)
            {
                _context.Enrollment.Remove(enrollment);
                await SaveChangesAsync();
            }
            return enrollment;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
