using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMNAT.School.Models;
using Microsoft.EntityFrameworkCore;

namespace IMNAT.School.Repositories
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<Students> Students { get; set; }
        public DbSet<Enrollments> Enrollments { get; set; }
        public DbSet<Courses> Courses { get; set; }

    }
}
