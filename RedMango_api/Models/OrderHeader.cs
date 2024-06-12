using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedMango_api.Models
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }
        [Required]
        public string PickupName { get; set; }

        [Required]
        public string PickupEmail { get; set; }
        [Required]
        public string PickupPhone { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser User { get; set; }

        public double OrderTotal { get; set; }

        public DateTime orderDate { get; set; }
        public string StripePaymentIntentId { get; set; }

        public string Status { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }


    }
}
