using Microsoft.EntityFrameworkCore;
using wowberry_crud.Models;

namespace wowberry_crud.Data
{
    public class ApplicationDbContext : DbContext
    {      
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Database Tables
        public DbSet<Users> users { get; set; }
        public DbSet<Entries> entries { get; set; }
        public DbSet<Audit> audits { get; set; }


        // Audit 
        public int ModifiedByUserId { get; set; } 

        public override int SaveChanges()
        {
            var auditEntries = BeforeSaveChanges();
            var result = base.SaveChanges();

            if (auditEntries.Any())
            {
                audits.AddRange(auditEntries);
                base.SaveChanges();
            }

            return result;
        }

        private List<Audit> BeforeSaveChanges()
        {
            var auditEntries = new List<Audit>();

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
            {
                var tableName = entry.Entity.GetType().Name;
                foreach (var property in entry.OriginalValues.Properties)
                {
                    var originalValue = entry.OriginalValues[property]?.ToString();
                    var currentValue = entry.CurrentValues[property]?.ToString();

                    if (originalValue != currentValue)
                    {
                        auditEntries.Add(new Audit
                        {
                            TableName = tableName,
                            FieldName = property.Name,
                            OldValue = originalValue,
                            NewValue = currentValue,
                            ModifiedBy = ModifiedByUserId,
                            ModifiedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            return auditEntries;
        }
    }
}
