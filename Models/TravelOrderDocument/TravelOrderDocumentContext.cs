using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelManagementApi.Models.TravelOrder;

namespace TravelManagementApi.Models.TravelOrderDocument
{
    public class TravelOrderDocumentContext : DbContext
    {
        public TravelOrderDocumentContext(DbContextOptions<TravelOrderDocumentContext> options)
    : base(options)
        {
        }

        public DbSet<TravelOrderDocumentItem> TravelOrderDocumentItems { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChangesAsync();
        }
    }
}
