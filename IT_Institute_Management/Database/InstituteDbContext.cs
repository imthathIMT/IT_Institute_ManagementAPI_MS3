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
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Announcement> Announcements { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships and constraints if necessary
            modelBuilder.Entity<Enrollment>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(e => e.StudentNIC);

            modelBuilder.Entity<Enrollment>()
                .HasOne<Course>()
                .WithMany()
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Payment>()
                .HasOne<Enrollment>()
                .WithMany()
                .HasForeignKey(p => p.EnrollmentId);

            modelBuilder.Entity<Address>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(a => a.StudentNIC);

            modelBuilder.Entity<Notification>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(n => n.StudentNIC);
        }
    }
}
