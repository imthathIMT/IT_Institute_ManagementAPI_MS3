using IT_Institute_Management.Entity;
using Microsoft.EntityFrameworkCore;

namespace IT_Institute_Management.Database
{
    public class InstituteDbContext : DbContext
    {
        public InstituteDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Notification> Notification { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships and constraints if necessary

            //ONE to ONE
            modelBuilder.Entity<Student>()
               .HasOne(a => a.Address)
               .WithOne(s => s.Student)
               .HasForeignKey<Address>(a => a.StudentNIC);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Enrollments)
                .WithOne(s=> s.Student)
                .HasForeignKey(e => e.StudentNIC);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Enrollments)
                .WithOne(c => c.Course)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Enrollment>()
               .HasMany(p => p.payments)
                .WithOne(e => e.Enrollment)
                .HasForeignKey(p => p.EnrollmentId);

           

            modelBuilder.Entity<Student>()
                .HasMany(n => n.Notification)
                .WithOne(s => s.Student)
                .HasForeignKey(n => n.StudentNIC);

         
        }
    }
}
