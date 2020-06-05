using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using IMNAT.School.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.EntityFrameworkCore;

namespace IMNAT.School.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //    .SetBasePath(Path.Combine(
            //    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            //    "C:\\Users\\alire\\source\\repos\\alireza260\\DevIntroduction\\IMNAT.School.CLI\\"))
            //    .AddJsonFile("appsettings.json");
            //IConfigurationRoot Configuration = config.Build();
            //var serviceProvider = new ServiceCollection()
            //.AddDbContext<Context>(o =>
            //o.UseSqlServer(Configuration.GetConnectionString("DBConnection")))
            //.BuildServiceProvider();
            //var context = serviceProvider.GetRequiredService<Context>();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            //var seedData = Configuration.GetSection("SeedData");
            //foreach (IConfigurationSection section in seedData.GetChildren())
            //{
            //    var id = section.GetValue<int>("CourseID");
            //    var description = section.GetValue<string>("CourseName");
            //    context.Courses.Add(new Courses { CourseName = description, });
            //    context.SaveChanges();
            //}

            // create service collection
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IAppServices, AppServices>();

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // DI section
            var controller = ActivatorUtilities.CreateInstance<Controller>(serviceProvider);

            // Initialize context
            controller.InitializeContext();

            Console.WriteLine("Please provide the required information for the new student.");
            Console.WriteLine("Enter the student's first name: ");
            var firstName = Console.ReadLine();
            Console.WriteLine("Enter the student's last name: ");
            var lastName = Console.ReadLine();
            Console.WriteLine("Enter the student's email: ");
            var email = Console.ReadLine();

            // Create and save a new Student
            controller.CreateStudent(firstName, lastName, email);

            // Display all Courses from the database
            controller.DisplayAndValidateCourses();

            // Create and save enrollment information for the Student based on the selected Course
            controller.CreateEnrollment();

            // Display enrolled students for each of the courses in the database
            Console.WriteLine("The following are the lists of enrolled students for each of the courses in the database: ");
            controller.DisplayEnrolledStudents();

            // Exit the Console App
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
