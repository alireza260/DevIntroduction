using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.EntityFrameworkCore;
using IMNAT.School.Repositories;

namespace IMNAT.School.Services
{
    public class AppServices : IAppServices
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CourseName { get; set; }
        public Context dbContext { get; set; }
        public AppRepository appRepository { get; set; }

        public void InitializeContext()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "C:\\Users\\alire\\source\\repos\\alireza260\\DevIntroduction\\IMNAT.School.CLI\\"))
                .AddJsonFile("appsettings.json");
            IConfigurationRoot Configuration = config.Build();
            var serviceProvider = new ServiceCollection()
            .AddDbContext<Context>(o =>
            o.UseSqlServer(Configuration.GetConnectionString("DBConnection")))
            .BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<Context>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var seedData = Configuration.GetSection("SeedData");
            appRepository = new AppRepository();
            foreach (IConfigurationSection section in seedData.GetChildren())
            {
                var id = section.GetValue<int>("CourseID");
                var description = section.GetValue<string>("CourseName");
                dbContext = appRepository.AddCourses(context, description);
                dbContext.SaveChanges();
            }
        }

        public void CreateStudent(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            dbContext = appRepository.AddStudent(dbContext, firstName, lastName, email);
            dbContext.SaveChanges();
        }

        public void CreateEnrollment()
        {
            dbContext = appRepository.AddEnrollment(dbContext, FirstName, LastName, CourseName);
            dbContext.SaveChanges();
        }

        public void DisplayAndValidateCourses()
        {
            var courses = appRepository.GetAvailableCourses(dbContext);

            Console.WriteLine("The following are all the available courses:");
            foreach (var item in courses)
            {
                Console.WriteLine(item.CourseName);
            }

            Console.WriteLine("Please enter the name of the selected course for the new student: ");
            var courseName = Console.ReadLine();

            while (true)
            {
                var exists = false;
                foreach (var item in courses)
                {
                    if (item.CourseName == courseName)
                    {
                        exists = true;
                    }
                }
                if (exists)
                {
                    break;
                }
                Console.WriteLine("Please enter the correct name of the selected course for the new student: ");
                courseName = Console.ReadLine();
            }
            CourseName = courseName;
        }

        public void DisplayEnrolledStudents()
        {
            var res = appRepository.GetAvailableCourses(dbContext);
            var jointQuery = appRepository.GetEnrolledStudents(dbContext);

            foreach (var course in res)
            {
                Console.WriteLine(course.CourseName);
                Console.WriteLine("---------------------");
                Console.WriteLine();
                foreach (var item in jointQuery)
                {
                    if (course.CourseID == item.courseID)
                    {
                        Console.WriteLine(item.studentFirstName + " " + item.studentLastName);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
