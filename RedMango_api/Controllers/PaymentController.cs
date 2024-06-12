using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedMango_api.Data;
using RedMango_api.Models;
using Stripe;
using System.Net;

namespace RedMango_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        public PaymentController( IConfiguration configuration, AppDbContext context)
        {
            _response = new();
            _configuration = configuration;
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> MakePayment (string userId)
        {
            ShoppingCart shoppingCart = _context.shoppingCarts.Include(u => u.CartItems)
                .ThenInclude(u => u.MenuItem).FirstOrDefault(u => u.UserId == userId);
            if (shoppingCart == null || shoppingCart.CartItems == null || shoppingCart.CartItems.Count == 0) 
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response); 
            }
            #region Create Payment Intent
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            double carttotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);

            var options = new PaymentIntentCreateOptions
            {
                Amount =(int)(carttotal*100),
                Currency = "usd",
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
            };
            PaymentIntentService service = new();
            PaymentIntent response = service.Create(options);

            shoppingCart.StripePaymentIntentId = response.Id;
            shoppingCart.ClientSecret = response.ClientSecret;
              
            #endregion

            _response.Result = shoppingCart;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);

        }
    }
}
