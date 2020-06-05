using System;
using System.Collections.Generic;
using IMNAT.School.Services;
using System.Text;

namespace IMNAT.School.CLI
{
    class Controller
    {
        IAppServices _services;

        public Controller(IAppServices services)
        {
            _services = services;
        }

        public void InitializeContext()
        {
            _services.InitializeContext();
        }

        public void CreateStudent(string firstName, string lastName, string email)
        {
            _services.CreateStudent(firstName, lastName, email);
        }

        public void CreateEnrollment()
        {
            _services.CreateEnrollment();
        }

        public void DisplayAndValidateCourses()
        {
            _services.DisplayAndValidateCourses();
        }

        public void DisplayEnrolledStudents()
        {
            _services.DisplayEnrolledStudents();
        }
    }
}
