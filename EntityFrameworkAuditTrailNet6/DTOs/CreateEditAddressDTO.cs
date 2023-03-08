using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkAuditTrailNet6.DTOs
{
    public class CreateEditAddressDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string AddressLine1 { get; set; }

        [StringLength(30)]
        public string AddressLine2 { get; set; }

        [StringLength(30)]
        public string AddressLine3 { get; set; }

        [Required]
        [StringLength(30)]
        public string City { get; set; }

        [StringLength(30)]
        public string County { get; set; }

        [Required]
        [StringLength(20)]
        public string Postcode { get; set; }
    }
}
