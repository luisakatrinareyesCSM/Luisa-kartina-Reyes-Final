using System.ComponentModel.DataAnnotations.Schema;

namespace Kat.Infrastructure.Domain.Models
{
    public class User
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public Guid? RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role? Role { get; set; }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
