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
        public DbSet<Group> Groups { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Departmnet> Departmnets { get; set; }
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
                    .HasDefaultValue(new DateTime());
                user.Property(u => u.Role).HasDefaultValue(Roles.STUDENT);

                user.Property(u => u.Password).IsRequired();

                user.Property(u => u.Username)
                    .HasMaxLength(24)
                    .IsRequired();
                user.HasIndex(u => u.Username).IsUnique();

                user.Property(u => u.Email)
                    .HasMaxLength(50)
                    .IsRequired();
                user.HasIndex(u => u.Email).IsUnique();

                user.Property(u => u.Name)
                    .HasMaxLength(24)
                    .IsRequired();

                user.Property(u => u.Surname)
                    .HasMaxLength(24)
                    .IsRequired();

                user.HasData(new User
                {
                    Name = "admin",
                    Surname = "admin",
                    Password = "admin",
                    Email = "ADmin",
                    Username = "admin",
                    Role = Roles.ADMIN
                });
            });

            modelBuilder.Entity<Departmnet>(department =>
            {
                department.HasKey(d => d.Id);
                department.Property(d => d.Id).HasDefaultValueSql("NEWID()");
            });

            modelBuilder.Entity<Group>(group =>
            {
                group.HasKey(g => g.Id);
                group.Property(g => g.Id).HasDefaultValueSql("NEWID()");
            });

            modelBuilder.Entity<Faculty>(faculty =>
            {
                faculty.HasKey(f => f.Id);
                faculty.Property(f => f.Id).HasDefaultValueSql("NEWID()");
            });

            modelBuilder.Entity<Schedule>(schedule =>
            {
                schedule.HasKey(s => s.Id);
                schedule.Property(s => s.Id).HasDefaultValueSql("NEWID()");
            });
        }
    }
}
