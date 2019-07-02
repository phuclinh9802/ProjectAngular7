using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlTypes;
using System.ComponentModel.DataAnnotations;

namespace RegisterLogin.Models
{
    public class RegisterContext : IdentityDbContext<Register>
    {

        public RegisterContext(DbContextOptions<RegisterContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = "Data Source=registered.db";
                optionsBuilder.UseSqlite(connectionString);
            }
        }
        public DbSet<Register> Register { get; set;  }
        public DbSet<Class> Class { get; set; }
        public DbSet<Student> Student { get; set; }
    }
    public class Register : IdentityUser
    {
        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string Role { get; set; }
    }
    public class UserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class AppSettings
    {
        public string JWT_SECRET { get; set; }
        public string Client_URL { get; set; }
    }
    public class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
    public class Class
    {
        
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public string Location { get; set; }

        public IList<Student> Student { get; set; }
    }
    public class Student
    {
        
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; }
        public double Gpa { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public Guid ClassId { get; set; }
        public Class Class { get; set; }
    }
}
