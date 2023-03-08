using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkAuditTrailNet6.DTOs
{
    public class CreateEditContactDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(30)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
