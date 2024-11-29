using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.IRepositories;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

namespace IT_Institute_Management.Repositories
{
    public class StudentMessageRepository : IStudentMessageRepository
    {
        private readonly InstituteDbContext _context;
        private readonly IStudentRepository _studentRepository;

        public StudentMessageRepository(InstituteDbContext context, IStudentRepository studentRepository)
        {
            _context = context;
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<StudentMessage>> GetAllAsync()
        {
            var data = await _context.StudentMessages
                                 .Include(sm => sm.Student)
                                 .ThenInclude(s => s.Address)
                                 .ToListAsync();

            if (data != null)
            {
                return data;
            }
            else
            {
                throw new NotFoundException("Student messages not found");
            }
        }

        public async Task<IEnumerable<StudentMessage>> GetByStudentNICAsync(string studentNIC)
        {
            var data =  await _context.StudentMessages
                                 .Where(sm => sm.StudentNIC == studentNIC)
                                 .Include(sm => sm.Student)
                                 .ThenInclude(s => s.Address)
                                 .ToListAsync();

            if (data != null)
            {
                return data;
            }
            else
            {
                throw new NotFoundException("Student messages not found");
            }
        }

        public async Task<StudentMessage> AddAsync(StudentMessage studentMessage)
        {
            // Await the asynchronous call to get the student by NIC
            var student = await _studentRepository.GetByNicAsync(studentMessage.StudentNIC!);

            // Check if the student exists
            if (student != null)
            {
                // Add the message to the context and save it
                var data = await _context.StudentMessages.AddAsync(studentMessage);
                await _context.SaveChangesAsync();

                // Return the entity after saving it
                return data.Entity;
            }
            else
            {
                throw new Exception($"Student not found with NIC: {studentMessage.StudentNIC}");
            }
            
        }

        public async Task DeleteAsync(Guid id)
        {
            var message = await _context.StudentMessages.FindAsync(id);
            if (message != null)
            {
                _context.StudentMessages.Remove(message);
            }
        }

        public async Task<bool> SaveAsync()
        {
            // Save the changes to the database and return whether the save was successful
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
