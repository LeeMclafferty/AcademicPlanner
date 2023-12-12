using SQLite;
using System.ComponentModel.DataAnnotations;

namespace AcademicPlanner.CoreEntities
{
    // All the code in this file is included in all platforms.
    public partial class Term
    {        
        [PrimaryKey, AutoIncrement]
        public int TermId { get; set; }
        [Required]
        public string TermTitle { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}