using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataLayer.Contexts
{
    public class AcademyContext : DbContext
    {
        public AcademyContext() { }
        public AcademyContext(DbContextOptions<AcademyContext> opts) : base(opts) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Department> Departmnets { get; set; }
        public DbSet<Schedule> Schedule { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetConnectionString("Local");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.Id);
                user.Property(u => u.Id).HasDefaultValueSql("NEWID()");

                user.Property(u => u.IsEmailConfirmed)
                    .HasDefaultValue(false);
                user.Property(u => u.RefreshToken)
                    .HasDefaultValue(string.Empty);
                user.Property(u => u.RefreshTokenExpiryTime)
                    .HasDefaultValue(DateTime.UtcNow);
                user.Property(u => u.Role).HasDefaultValue(Roles.STUDENT);

                user.Property(u => u.PasswordHash).IsRequired();

                user.Property(u => u.Username)
                    .HasMaxLength(24)
                    .IsRequired();
                user.HasIndex(u => u.Username).IsUnique();

                user.Property(u => u.Email)
                    .HasMaxLength(50)
                    .IsRequired();
                user.HasIndex(u => u.Email).IsUnique();

                user.Property(u => u.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                user.Property(u => u.LastName)
                    .IsRequired()
                    .HasMaxLength(50);


                user.HasData(new User
                {
                    Id = Guid.Parse("b68d2dfc-8a9d-4e14-b1a4-6fc5c56f9e17"),
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("admin"),
                    Email = "admin@academy.com",
                    Username = "admin",
                    FirstName = "Admin",
                    LastName = "Admin",
                    Role = Roles.ADMIN,
                    IsEmailConfirmed = true,
                    RefreshToken = string.Empty,
                    RefreshTokenExpiryTime = DateTime.UtcNow
                });
            });

            modelBuilder.Entity<Student>(student =>
            {
                student.HasKey(s => s.Id);
                student.Property(s => s.Id).HasDefaultValueSql("NEWID()");

                student
                    .HasOne(s => s.Group)
                    .WithMany(g => g.Students)
                    .HasForeignKey(s => s.GroupId);

                student
                    .HasOne(s => s.User)
                    .WithOne()
                    .HasForeignKey<Student>(s => s.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Teacher>(teacher =>
            {
                teacher.HasKey(t => t.Id);
                teacher.Property(t => t.Id).HasDefaultValueSql("NEWID()");

                teacher
                    .HasMany(t => t.Groups)
                    .WithOne(g => g.Teacher)
                    .HasForeignKey(g => g.TeacherId);

                teacher
                    .HasOne(t => t.Department)
                    .WithMany(d => d.Teachers)
                    .HasForeignKey(t => t.DepartmentId);

                teacher
                    .HasOne(t => t.User)
                    .WithOne()
                    .HasForeignKey<Teacher>(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Department>(department =>
            {
                department.HasKey(d => d.Id);
                department.Property(d => d.Id).HasDefaultValueSql("NEWID()");

                department.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(50);
                department.HasIndex(d => d.Name).IsUnique();

                department
                    .HasMany(d => d.Teachers)
                    .WithOne(t => t.Department)
                    .HasForeignKey(t => t.DepartmentId);
            });

            modelBuilder.Entity<Group>(group =>
            {
                group.HasKey(g => g.Id);
                group.Property(g => g.Id).HasDefaultValueSql("NEWID()");

                group
                    .HasMany(g => g.Students)
                    .WithOne(s => s.Group)
                    .HasForeignKey(s => s.GroupId);

                group
                    .HasOne(g => g.Teacher)
                    .WithMany(t => t.Groups)
                    .HasForeignKey(g => g.TeacherId);

                group
                    .HasOne(g => g.Faculty)
                    .WithMany(f => f.Groups)
                    .HasForeignKey(g => g.FacultyId);
            });

            modelBuilder.Entity<Faculty>(faculty =>
            {
                faculty.HasKey(f => f.Id);
                faculty.Property(f => f.Id).HasDefaultValueSql("NEWID()");

                faculty
                    .HasMany(f => f.Groups)
                    .WithOne(g => g.Faculty)
                    .HasForeignKey(g => g.FacultyId);
            });

            modelBuilder.Entity<Schedule>(schedule =>
            {
                schedule.HasKey(s => s.Id);
                schedule.Property(s => s.Id).HasDefaultValueSql("NEWID()");

                schedule
                    .HasOne(s => s.Teacher)
                    .WithMany()
                    .HasForeignKey(t => t.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);

                schedule
                    .HasOne(s => s.Group)
                    .WithMany()
                    .HasForeignKey(s => s.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
