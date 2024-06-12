using Microsoft.EntityFrameworkCore;
using SimpleASPNetMVC.DbModels;

namespace SimpleASPNetMVC.Data
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<StudentFund> Funds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Student")
                .HasIndex(s => s.Email)
                .IsUnique();

            modelBuilder.Entity<StudentFund>().ToTable("StudentFund")
                .HasOne(e => e.Student)
                .WithOne(e => e.Fund)
                .HasForeignKey<StudentFund>(e => e.StudentId)
                .IsRequired();
        }

    }
}
