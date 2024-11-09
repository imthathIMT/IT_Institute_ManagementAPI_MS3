using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace IT_Institute_Management.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly InstituteDbContext _context;

        public StudentRepository(InstituteDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            try
            {
                return await _context.Students.Include(s => s.Address).Include(e => e.Enrollment).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all students: {ex.Message}");
                throw new ApplicationException("An error occurred while fetching the students list.");
            }
        }

        public async Task<Student> GetByNicAsync(string nic)
        {
            try
            {
                return await _context.Students.Include(s => s.Address).FirstOrDefaultAsync(s => s.NIC == nic);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching student with NIC {nic}: {ex.Message}");
                throw new ApplicationException($"An error occurred while fetching the student with NIC {nic}.");
            }
        }

        public async Task AddAsync(Student student)
        {
            try
            {
                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding student: {ex.Message}");
                throw new ApplicationException("An error occurred while adding the student.");
            }
        }

        public async Task UpdateAsync(Student student)
        {
            try
            {
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating student: {ex.Message}");
                throw new ApplicationException("An error occurred while updating the student.");
            }
        }

        public async Task DeleteAsync(string nic)
        {
            try
            {
                var student = await GetByNicAsync(nic);
                if (student == null)
                {
                    throw new ApplicationException($"Student with NIC {nic} not found.");
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting student with NIC {nic}: {ex.Message}");
                throw new ApplicationException($"An error occurred while deleting the student with NIC {nic}.");
            }
        }
    }
}
