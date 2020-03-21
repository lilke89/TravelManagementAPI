using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TravelManagementApi.Models
{
    public class TravelOrderItem : BaseEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}
