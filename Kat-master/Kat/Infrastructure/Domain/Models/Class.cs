using System.ComponentModel.DataAnnotations.Schema;

namespace Kat.Infrastructure.Domain.Models
{
    public class Class
    {
        public Guid? ClassId { get; set; }
        public string? Code { get; set; }
        public string ? YearLevel { get; set; }
        public DateTime? StartDate { get; set; }
        public Guid? CourseId { get; set; }
        public Meeting? Meeting { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

    }
    public enum Meeting
    {
        MWF = 0,
        TTT = 1,
        SAT = 2
    }
}
