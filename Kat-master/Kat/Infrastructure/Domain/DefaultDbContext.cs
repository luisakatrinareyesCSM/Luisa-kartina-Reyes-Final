using Kat.Infrastructure.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Kat.Infrastructure.Domain
{
    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
          : base(options)
        {
        }
        public DbSet<Class> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Class> classes = new List<Class>();
            List<Course> courses = new List<Course>();
            List<Role> roles = new List<Role>();
            List<User> users = new List<User>();
            List<UserLogin> userLogins = new List<UserLogin>();
            List<UserRole> userRoles = new List<UserRole>();


            roles.Add(new Role()
            {
                Id = Guid.Parse("00965ecf-acae-46fe-8775-d7834b07fd96"),
                Name = "Staff",
                Description = "People who work in school but not teaching",
                Abbreviation = "Stf"
            });

            roles.Add(new Role()
            {
                Id = Guid.Parse("1fb7085a-762f-440c-87de-59f75f85e934"),
                Name = "Admin",
                Description = "Admin",
                Abbreviation = "Adm"
            });

            modelBuilder.Entity<Role>().HasData(roles);

            // USERRRRRRRRRRRRRRR
            users.Add(new User()
            {
                Id = Guid.Parse("1d72f000-dbbd-419b-8af2-f571e1486ac0"),
                Name = "Reniel Mallari David",
                DateOfBirth = DateTime.Now,
                EmailAddress = "Reniel@mailinator.com",
                Gender = Gender.Male,
                RoleId = Guid.Parse("00965ecf-acae-46fe-8775-d7834b07fd96")
            });

            userLogins.AddRange(new List<UserLogin>()
            {
                new UserLogin()
                {
                    Id = Guid.NewGuid(),
                    UserId =Guid.Parse("1d72f000-dbbd-419b-8af2-f571e1486ac0"),
                    Type = "General",
                    Key = "Password",
                    Value = BCrypt.Net.BCrypt.EnhancedHashPassword("123")
                },
                new UserLogin()
                {
                    Id = Guid.NewGuid(),
                    UserId =Guid.Parse("1d72f000-dbbd-419b-8af2-f571e1486ac0"),
                    Type = "General",
                    Key = "IsActive",
                    Value = "true"
                },
                new UserLogin()
                {
                    Id = Guid.NewGuid(),
                    UserId =Guid.Parse("1d72f000-dbbd-419b-8af2-f571e1486ac0"),
                    Type = "General",
                    Key = "LoginRetries",
                    Value = "0"
                }
            });

            userRoles.Add(new UserRole()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("1d72f000-dbbd-419b-8af2-f571e1486ac0"),
                RoleId = Guid.Parse("00965ecf-acae-46fe-8775-d7834b07fd96"),
            });




            users.Add(new User()
            {
                Id = Guid.Parse("1d72f000-dbbd-419b-8af2-f571e1486ac1"),
                Name = "Clarissa Joy Flore",
                DateOfBirth = DateTime.Now,
                EmailAddress = "joy@mailinator.com",
                Gender = Gender.Female,
                RoleId = Guid.Parse("1fb7085a-762f-440c-87de-59f75f85e93")
            });

            userLogins.AddRange(new List<UserLogin>()
            {
                new UserLogin()
                {
                    Id = Guid.NewGuid(),
                    UserId =Guid.Parse("1d72f000-dbbd-419b-8af2-f571e1486ac1"),
                    Type = "General",
                    Key = "Password",
                    Value = BCrypt.Net.BCrypt.EnhancedHashPassword("123")
                },
                new UserLogin()
                {
                    Id = Guid.NewGuid(),
                    UserId =Guid.Parse("1d72f000-dbbd-419b-8af2-f571e1486ac1"),
                    Type = "General",
                    Key = "IsActive",
                    Value = "true"
                },
                new UserLogin()
                {
                    Id = Guid.NewGuid(),
                    UserId =Guid.Parse("1d72f000-dbbd-419b-8af2-f571e1486ac1"),
                    Type = "General",
                    Key = "LoginRetries",
                    Value = "0"
                }
            });

            userRoles.Add(new UserRole()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("1d72f000-dbbd-419b-8af2-f571e1486ac1"),
                RoleId = Guid.Parse("1fb7085a-762f-440c-87de-59f75f85e93"),
            });

            classes.Add(new Class()
            {
                ClassId = Guid.Parse("0c05f1c2-7e73-417d-8e8a-4e6e7ccedac5"),
                Code = "123",
                YearLevel ="123",
                StartDate = DateTime.Now,
                Meeting = Meeting.SAT,
                CourseId = Guid.Parse("9bd7bd29-5457-49eb-bc16-baade192f5b7")

            });


            courses.Add(new Course()
            {
                CourseId = Guid.Parse("9bd7bd29-5457-49eb-bc16-baade192f5b7"),
                Tittle="13",
                Description ="123",
                Abbrevitation= "123"
                
            });

            classes.Add(new Class()
            {
                ClassId = Guid.Parse("0012589e-27af-4c86-acdc-3c45cf27c956"),
                Code = "456",
                YearLevel = "456",
                StartDate = DateTime.Now,
                Meeting = Meeting.SAT,
                CourseId = Guid.Parse("1725d958-ca02-4e75-bda0-1671f73769bf")

            });

            courses.Add(new Course()
            {
                CourseId = Guid.Parse("1725d958-ca02-4e75-bda0-1671f73769bf"),
                Tittle = "456",
                Description = "456",
                Abbrevitation = "456"

            });




            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<UserLogin>().HasData(userLogins);
            modelBuilder.Entity<UserRole>().HasData(userRoles);
            modelBuilder.Entity<Role>().HasData(roles);
            modelBuilder.Entity<Class>().HasData(classes);
            modelBuilder.Entity<Course>().HasData(courses);
        }

    }
}
