using SQLite;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicPlanner.CoreEntities
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseId { get; set; }

        [Required]
        public int TermId { get; set; } // Foreign key, library does not support this natively.

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; } // Anticipated end date

        [Required]
        public string InstuctorEmail { get; set; }

        [Required]
        public string InstructorName { get; set; }

        [Required]
        public string InstuctorPhone { get; set; }

        [Required]
        public string Notes { get; set; }

        [Required]
        public string CourseStatus { get; set; }

        [Required]
        public string CourseNotes { get; set; }

        //         •   course name
        // 
        //         •   start and anticipated end dates(using a date picker)
        // 
        //         •   course status(using a picker)
        // 
        //         •   course instructor’s information: name, phone, and email
        // 
        //         •   add, share, and display optional notes
        // 
        //         •   set notifications for the start and end dates of each course
        // 
        //         •   display of a detailed view of each course, including the due date
        // 
        //         •   editing of the detailed course view screen
    }
}
