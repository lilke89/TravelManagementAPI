using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
