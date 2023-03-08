using EntityFrameworkAuditTrailNet6.Entities;
using EntityFrameworkAuditTrailNet6.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityFrameworkAuditTrailNet6.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        AuthenticationStateProvider authenticationStateProvider;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            AuthenticationStateProvider authenticationStateProvider)
            : base(options)
        {
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ChangeAudit> ChangeAudits { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await AuditChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task AuditChanges()
        {
            DateTime now = DateTime.Now;

            var entityEntries = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added ||
                            x.State == EntityState.Modified ||
                            x.State == EntityState.Deleted).ToList();

            foreach (EntityEntry entityEntry in entityEntries)
            {
                IncrementVersionNumber(entityEntry);

                if (entityEntry.Entity is IAuditable)
                    await CreateAuditAsync(entityEntry, now);
            }
        }

        private void IncrementVersionNumber(EntityEntry entityEntry)
        {
            if (entityEntry.Metadata.FindProperty("Version") != null)
            {
                var currentVersionNumber = Convert.ToInt32(entityEntry.CurrentValues["Version"]);
                entityEntry.CurrentValues["Version"] = currentVersionNumber + 1;
            }
        }

        private async Task CreateAuditAsync(EntityEntry entityEntry, DateTime timeStamp)
        {
            if (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Deleted)
            {
                var changeAudit = await GetChangeAuditAsync(entityEntry, timeStamp);
                await ChangeAudits.AddAsync(changeAudit);
            }
            else
            {
                foreach (var prop in entityEntry.OriginalValues.Properties)
                {
                    var originalValue = !string.IsNullOrWhiteSpace(entityEntry.OriginalValues[prop]?.ToString()) ?
                        entityEntry.OriginalValues[prop]?.ToString() : null;

                    var currentValue = !string.IsNullOrWhiteSpace(entityEntry.CurrentValues[prop]?.ToString()) ?
                        entityEntry.CurrentValues[prop]?.ToString() : null;

                    if (originalValue != currentValue)
                    {
                        var changeAudit = await GetChangeAuditAsync(entityEntry, timeStamp);
                        changeAudit.PropertyName = prop.Name;
                        changeAudit.OldValue = originalValue;
                        changeAudit.NewValue = currentValue;
                        await ChangeAudits.AddAsync(changeAudit);
                    }
                }
            }
        }

        private async Task<ChangeAudit> GetChangeAuditAsync(EntityEntry entityEntry, DateTime timeStamp)
        {
            return new ChangeAudit
            {
                EntityName = entityEntry.Entity.GetType().Name,
                Action = entityEntry.State.ToString(),
                EntityIdentifier = GetEntityIdentifier(entityEntry),
                UserName = await GetUserNameAsync(),
                TimeStamp = timeStamp
            };
        }

        private static string GetEntityIdentifier(EntityEntry entityEntry)
        {
            if (entityEntry.Entity is IdentityUser)
                return $"{entityEntry.CurrentValues["UserName"]}";
            else if (entityEntry.Entity is Contact)
                return $"{entityEntry.CurrentValues["FirstName"]} {entityEntry.CurrentValues["LastName"]}";
            else if (entityEntry.Entity is Address)
                return $"{entityEntry.CurrentValues["AddressLine1"]}, {entityEntry.CurrentValues["Postcode"]}";

            return "None";
        }

        private async Task<string?> GetUserNameAsync()
        {
            var authenticationState = await authenticationStateProvider.GetAuthenticationStateAsync();

            if (authenticationState.User.Identity.IsAuthenticated)
                return authenticationState.User.Identity.Name;

            return null;
        }
    }
}