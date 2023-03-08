using EntityFrameworkAuditTrailNet6.Interfaces;

namespace EntityFrameworkAuditTrailNet6.Entities
{
    public class Contact : IAuditable
    {
        public int Id { get; set; }

        public int Version { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
