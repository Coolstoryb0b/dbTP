using DekanatUniversity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DekanatUniversity.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<AcademicGroup> AcademicGroups { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Curriculum> Curriculums { get; set; }
        public DbSet<Vedomost> Vedomosti { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Student>()
                .HasOne(s => s.AcademicGroup)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Vedomost>()
                .HasOne(v => v.AcademicGroup)
                .WithMany(g => g.Vedomosti)
                .HasForeignKey(v => v.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Vedomost>()
                .HasOne(v => v.Discipline)
                .WithMany(d => d.Vedomosti)
                .HasForeignKey(v => v.DisciplineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Исправленная связь Vedomost -> Teacher (по int Id)
            builder.Entity<Vedomost>()
                .HasOne(v => v.Teacher)
                .WithMany(t => t.Vedomosti)
                .HasForeignKey(v => v.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Grade>()
                .HasOne(g => g.Vedomost)
                .WithMany(v => v.Grades)
                .HasForeignKey(g => g.VedomostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Curriculum>()
                .HasOne(c => c.Discipline)
                .WithMany(d => d.Curriculums)
                .HasForeignKey(c => c.DisciplineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}