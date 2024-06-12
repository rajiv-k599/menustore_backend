using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RedMango_api.Models.Dto
{
    public class OrderDetailsCreateDto
    {

     
        [Required]
        public int MenuItemId { get; set; }
       
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public Double Price { get; set; }


    }
}
