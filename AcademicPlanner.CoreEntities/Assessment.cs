using SQLite;
using System.ComponentModel.DataAnnotations;

namespace AcademicPlanner.CoreEntities
{
    public class Assessment
    {
        [AutoIncrement, PrimaryKey]
        public int assessmentId { get; set; }

        [Required]
        public int courseId { get; set; }

        [Required]
        public string assessmentName { get; set; }

        [Required]
        public string assessmentType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
    }
}
