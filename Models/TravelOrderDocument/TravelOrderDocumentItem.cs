using System.ComponentModel.DataAnnotations.Schema;

namespace TravelManagementApi.Models.TravelOrder
{
    public class TravelOrderDocumentItem : BaseEntity
    {
        public long Id { get; set; }

        [ForeignKey("TravelOrderListItem")]
        public long ListId { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}
