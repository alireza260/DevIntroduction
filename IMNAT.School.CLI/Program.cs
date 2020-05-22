using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using IMNAT.School.Database;
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
            foreach (IConfigurationSection section in seedData.GetChildren())
            {
                var id = section.GetValue<int>("CourseID");
                var description = section.GetValue<string>("CourseName");
                context.Courses.Add(new Courses { CourseName = description, });
                context.SaveChanges();
            }

            using (var db = context)
            {
                // Create and save a new Student
                Console.WriteLine("Please provide the required information for the new student.");
                Console.WriteLine("Enter the student's first name: ");
                var firstName = Console.ReadLine();
                Console.WriteLine("Enter the student's last name: ");
                var lastName = Console.ReadLine();
                Console.WriteLine("Enter the student's email: ");
                var email = Console.ReadLine();

                var student = new Students { FirstName = firstName, LastName = lastName, Email = email };
                db.Students.Add(student);
                db.SaveChanges();

                // Display all Courses from the database
                var res = (from cr in db.Courses
                           select cr).Distinct();

                Console.WriteLine("The following are all the available courses:");
                foreach (var item in res)
                {
                    Console.WriteLine(item.CourseName);
                }

                // Create and save enrollment information for the Student based on the selected Course
                Console.WriteLine("Please enter the name of the selected course for the new student: ");
                var courseName = Console.ReadLine();

                while (true)
                {
                    var exists = false;
                    foreach (var item in res)
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

                var courseQuery = (from cr in db.Courses
                                   where cr.CourseName == courseName
                                   select cr).First();

                var studentQuery = (from st in db.Students
                                    where st.FirstName == firstName && st.LastName == lastName
                                    select st).First();

                var enrollment = new Enrollments { CourseID = courseQuery.CourseID, StudentID = studentQuery.StudentID };
                db.Enrollments.Add(enrollment);
                db.SaveChanges();

                // Display enrolled students for each of the courses in the database
                Console.WriteLine("The following are the lists of enrolled students for each of the courses in the database: ");

                var jointQuery = (from cr in db.Courses
                                  join en in db.Enrollments on cr.CourseID equals en.CourseID
                                  join st in db.Students on en.StudentID equals st.StudentID
                                  select new
                                  {
                                      courseID = cr.CourseID,
                                      studentFirstName = st.FirstName,
                                      studentLastName = st.LastName
                                  }).ToList();

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

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
