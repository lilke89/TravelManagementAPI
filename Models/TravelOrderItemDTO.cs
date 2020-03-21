using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TravelManagementApi.Models
{
    public class TravelOrderItemDTO
    {
        [FromForm(Name = "document")]
        public IFormFile Document { get; set; }
    }
}
