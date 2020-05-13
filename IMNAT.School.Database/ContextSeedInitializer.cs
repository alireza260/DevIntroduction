using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace IMNAT.School.Database
{
    public class ContextSeedInitializer : DropCreateDatabaseAlways<Context>
    {
        protected override void Seed(Context context)
        {
            var courses = new List<Courses>
            {
            new Courses{CourseID=1,CourseName="English 101",},
            new Courses{CourseID=2,CourseName="French 101",},
            new Courses{CourseID=3,CourseName="Computer Science",}
            };
            courses.ForEach(s => context.Courses.Add(s));
            context.SaveChanges();
        }
    }
}
