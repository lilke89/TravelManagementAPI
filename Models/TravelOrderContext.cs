using Microsoft.EntityFrameworkCore;

namespace TravelManagementApi.Models
{
    public class TravelOrderContext : DbContext
    {
        public TravelOrderContext(DbContextOptions<TravelOrderContext> options) : base(options)
        {
            
        }

        public DbSet<TravelOrderItem> TravelOrderItems { get; set; }
    }
}
