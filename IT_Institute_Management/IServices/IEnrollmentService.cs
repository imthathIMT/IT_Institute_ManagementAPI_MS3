﻿using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IServices
{
    public interface IEnrollmentService
    {
        Task<Enrollment> CreateEnrollmentAsync(EnrollmentRequestDto enrollmentRequest);
        Task<Enrollment> DeleteEnrollmentByIdAsync(Guid id, bool forceDelete = false);
        Task<Enrollment> UpdateEnrollmentCompletionStatus(Guid id);
        Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync();
        Task<Enrollment> UpdateEnrollmentDataAsync(Guid id, EnrollmentRequestDto enrollmentRequest);
        Task<Enrollment> GetEnrollmentByIdAsync(Guid id);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByNICAsync(string nic);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByCompletionStatusAsync(bool isComplete);

        Task<IEnumerable<Enrollment>> GetEnrollmentsByNICAndCompletionStatusAsync(string nic, bool isComplete);
    }

}

