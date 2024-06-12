using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RedMango_api.Models.Dto
{
    public class OrderHeaderUpdateDto
    {
        
        public int OrderHeaderId { get; set; }
   
        public string PickupName { get; set; }

        public string PickupEmail { get; set; }
      
        public string PickupPhone { get; set; }



        public string StripePaymentIntentId { get; set; }

        public string Status { get; set; }
     
    }
}
