namespace EntityFrameworkAuditTrailNet6.Entities
{
    public class ChangeAudit
    {
        public int Id { get; set; }

        public string EntityName { get; set; }

        public string Action { get; set; }

        public string EntityIdentifier { get; set; }

        public string? PropertyName { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public string? UserName { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
