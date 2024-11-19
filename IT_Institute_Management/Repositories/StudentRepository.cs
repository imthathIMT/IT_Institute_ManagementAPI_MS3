using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
                return await _context.Students.Include(s => s.Address).Include(e => e.Enrollment).Include(n => n.Notification).Include(e => e.Enrollment).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the students list.");
            }
        }

        public async Task<Student> GetByNicAsync(string nic)
        {
            try
            {
                return await _context.Students.Include(s => s.Address).Include(e => e.Enrollment).Include(n=>n.Notification).FirstOrDefaultAsync(s => s.NIC == nic);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching the student with NIC {nic}.");
            }
        }

        public async Task AddAsync(Student student)
        {
            try
            {
                
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(student);
                bool isValid = Validator.TryValidateObject(student, validationContext, validationResults, true);

                if (!isValid)
                {
                   
                    var errorMessages = validationResults.Select(vr => vr.ErrorMessage).ToList();
                    throw new ValidationException(string.Join(", ", errorMessages));
                }

              
                var user = new User()
                {
                    NIC = student.NIC,
                    Password = student.Password,
                    Role = Role.Student
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync(); 

               
                student.UserId = user.Id;

                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
            }
            catch (ValidationException validationEx)
            {
                throw new Exception($"Validation failed: {validationEx.Message}");
            }
            catch (Exception ex)
            {
               

                throw new Exception($"An error occurred while adding the student: {ex.Message}", ex);
            }
        }

        public async Task UpdateAsync(Student student)
        {
            try
            {
                
                var user = new User()
                {
                    NIC = student.NIC,
                    Password = student.Password,
                    Role = Role.Student
                };
                _context.Users.Update(user);
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {            
                throw new Exception("An error occurred while updating the student.");
            }
        }

        public async Task DeleteAsync(string nic)
        {
            try
            {
                var student = await GetByNicAsync(nic);
                if (student == null)
                {
                    throw new Exception($"Student with NIC {nic} not found.");
                }

                
                _context.Students.Remove(student);

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.NIC == student.NIC);

                if (user != null)
                {
                    _context.Users.Remove(user);
                }

               
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
               
                throw new Exception($"The entity was modified or deleted by another user. Please try again. {ex.Message}");
            }
            catch (Exception ex)
            {
               
                throw new Exception($"An error occurred while deleting the student with NIC {nic}. {ex.Message}");
            }
        }

        public async Task<List<string>> GetAllStudentMail()
        {
            try
            {
                var studentEmails = await _context.Students
                                                  .Where(s => !string.IsNullOrEmpty(s.Email)) 
                                                  .Select(s => s.Email) 
                                                  .ToListAsync();

                return studentEmails;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while fetching the student emails.", ex);
            }
        }


    }
}
