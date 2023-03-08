using EntityFrameworkAuditTrailNet6.Interfaces;

namespace EntityFrameworkAuditTrailNet6.Entities
{
    public class Address: IAuditable
    {
        public int Id { get; set; }

        public string AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? AddressLine3 { get; set; }

        public string City { get; set; }

        public string? County { get; set; }

        public string Postcode { get; set; }
    }
}
