using System.ComponentModel.DataAnnotations;

namespace RedMango_api.Models.Dto
{
    public class OrderHeaderCreateDto
    {
        [Required]
        public string PickupName { get; set; }

       [Required]
        public string PickupEmail { get; set; }
        [Required]
        public string PickupPhone { get; set; }

        public string ApplicationUserId { get; set; }

        public double OrderTotal { get; set; }

        public string StripePaymentIntentId { get; set; }

        public string Status { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<OrderDetailsCreateDto> OrderDetails { get; set; }
    }
}
