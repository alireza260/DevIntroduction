using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Data.Entity;

namespace IMNAT.School.Database
{
    public class Courses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseID { get; set; }
        public string CourseName { get; set; }

        //navigation property
        public virtual ICollection<Enrollments> Enrollments { get; set; }
    }
}
