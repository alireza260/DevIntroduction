using System;
using System.Collections.Generic;
using System.Linq;
using IMNAT.School.Models;
using System.Text;

namespace IMNAT.School.Repositories
{
    public class AppRepository
    {
        public class StudentEnrollmentInfo
        {
            public int courseID { get; set; }
            public string studentFirstName { get; set; }
            public string studentLastName { get; set; }
        }

        public Context AddCourses(Context dbContext, string description)
        {
            dbContext.Courses.Add(new Courses { CourseName = description, });
            return dbContext;
        }

        public Context AddStudent(Context dbContext, string firstName, string lastName, string email)
        {
            var student = new Students { FirstName = firstName, LastName = lastName, Email = email };
            dbContext.Students.Add(student);
            return dbContext;
        }

        public Context AddEnrollment(Context dbContext, string firstName, string lastName, string courseName)
        {
            var courseQuery = (from cr in dbContext.Courses
                               where cr.CourseName == courseName
                               select cr).FirstOrDefault();

            var studentQuery = (from st in dbContext.Students
                                where st.FirstName == firstName && st.LastName == lastName
                                select st).FirstOrDefault();

            var enrollment = new Enrollments { CourseID = courseQuery.CourseID, StudentID = studentQuery.StudentID };
            dbContext.Enrollments.Add(enrollment);
            return dbContext;
        }

        public IQueryable<Courses> GetAvailableCourses(Context dbContext)
        {
            var res = (from cr in dbContext.Courses
                       select cr).Distinct();
            return res;
        }

        public List<StudentEnrollmentInfo> GetEnrolledStudents(Context dbContext)
        {
            var query = (from cr in dbContext.Courses
                              join en in dbContext.Enrollments on cr.CourseID equals en.CourseID
                              join st in dbContext.Students on en.StudentID equals st.StudentID
                              select new StudentEnrollmentInfo()
                              {
                                  courseID = cr.CourseID,
                                  studentFirstName = st.FirstName,
                                  studentLastName = st.LastName
                              }).ToList();
            return query;
        }
    }
}
