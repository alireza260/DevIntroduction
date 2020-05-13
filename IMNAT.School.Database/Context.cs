using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IMNAT.School.Database
{
    public class Context : DbContext
    {

        public Context(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Students> Students { get; set; }
        public DbSet<Enrollments> Enrollments { get; set; }
        public DbSet<Courses> Courses { get; set; }

    }
}
