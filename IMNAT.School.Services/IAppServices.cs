using System;
using System.Collections.Generic;
using System.Text;

namespace IMNAT.School.Services
{
    public interface IAppServices
    {
        void InitializeContext();
        void CreateStudent(string firstName, string lastName, string email);
        void CreateEnrollment();
        void DisplayAndValidateCourses();
        void DisplayEnrolledStudents();
    }
}
