using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Data.Entity;

namespace IMNAT.School.Database
{
    public class Enrollments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }

        //navigation properties
        [ForeignKey("CourseID")]
        public virtual Courses Course { get; set; }
        [ForeignKey("StudentID")]
        public virtual Students Student { get; set; }
    }
}
