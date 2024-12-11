using IT_Institute_Management.EmailSerivice;
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
        public DbSet<Enrollment> Enrollment { get; set; } 
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SocialMediaLinks> SocialMediaLinks { get; set; }
        public DbSet<StudentMessage> StudentMessages { get; set; }

        //email Template table
        public DbSet<EmailTemplate> EmailTemplates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<Student>()
               .HasOne(a => a.Address)
               .WithOne(s => s.Student)
               .HasForeignKey<Address>(a => a.StudentNIC);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Enrollment)
                .WithOne(s=> s.Student)
                .HasForeignKey(e => e.StudentNIC);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Enrollment)
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

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Enrollment)
                .WithMany(e => e.payments)
                .HasForeignKey(p => p.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SocialMediaLinks>()
           .HasOne(s => s.Student)
           .WithOne(u => u.SocialMediaLinks)
           .HasForeignKey<SocialMediaLinks>(s => s.StudentNIC)
            .OnDelete(DeleteBehavior.Cascade);

          
            modelBuilder.Entity<StudentMessage>()
                .HasOne(sm => sm.Student)
                .WithMany(s => s.StudentMessages)
                .HasForeignKey(sm => sm.StudentNIC)
                .OnDelete(DeleteBehavior.Cascade); 


            modelBuilder.Entity<Course>()
                    .Property(c => c.Fees)
                   .HasColumnType("decimal(18, 2)")  
                   .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                   .Property(p => p.Amount)
                   .HasColumnType("decimal(18, 2)")
                   .HasPrecision(18, 2);  

            modelBuilder.Entity<Payment>()
                .Property(p => p.DueAmount)
                .HasColumnType("decimal(18, 2)") 
                .HasPrecision(18, 2);  

            modelBuilder.Entity<Payment>()
                .Property(p => p.TotalPaidAmount)
                .HasColumnType("decimal(18, 2)") 
                .HasPrecision(18, 2);
        }
    }
}
