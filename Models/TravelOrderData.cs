using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TravelManagementApi.Models
{
    public class TravelOrderData
    {
        public string FileName { get; set; }

        public string Employee { get; set; }
        public string Initials { get; set; }
        public string InitialsShorthand { get; set; }
        public string OrderNumber { get; set; }
        public string Role { get; set; }
        public string City { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string DatePaid { get; set; }
        public string NumberOfDays { get; set; }
        public string AmountPerDay { get; set; }
        public string AmountSumForDays { get; set; }
    }
}
