﻿using IT_Institute_Management.Database;
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

        public async Task<IEnumerable<Enrollment>> GetEnrollmentByNICAsync(string nic)
    {
        return await _context.Enrollment
                              .Where(e => e.Student.NIC == nic)
                              .ToListAsync();
    }


        public async Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _context.Enrollment
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();
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



        public async Task<IEnumerable<Enrollment>> DeleteEnrollmentsByNICAsync(string nic)
        {
            var enrollments = await GetEnrollmentByNICAsync(nic);  
            if (enrollments != null && enrollments.Any())
            {
                _context.Enrollment.RemoveRange(enrollments); 
                await SaveChangesAsync();
            }
            return enrollments;
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByCompletionStatusAsync(bool isComplete)
        {
            return await _context.Enrollment
                .Where(e => e.IsComplete == isComplete)
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByNICAndCompletionStatusAsync(string nic, bool isComplete)
        {
            return await _context.Enrollment
                .Where(e => e.Student.NIC == nic && e.IsComplete == isComplete)
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }



    }
}
