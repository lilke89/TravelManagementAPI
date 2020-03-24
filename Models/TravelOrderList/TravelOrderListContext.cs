using Microsoft.EntityFrameworkCore;

namespace TravelManagementApi.Models
{
    public class TravelOrderListContext : DbContext
    {
        public TravelOrderListContext(DbContextOptions<TravelOrderListContext> options) : base(options)
        {
            
        }

        public DbSet<TravelOrderListItem> TravelOrderListItems { get; set; }
    }
}
