using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedMango_api.Data;
using RedMango_api.Models;
using RedMango_api.Services;
using System.Net;

namespace RedMango_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        private ApiResponse _response;
        public ShoppingCartController(AppDbContext context)
        {
            _context = context;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetShoppingCart(string userId)
        {
            try
            {
                ShoppingCart shoppingCart;
                if (string.IsNullOrEmpty(userId))
                {
                    shoppingCart = new();
                }
                else
                {
                    shoppingCart = _context.shoppingCarts.Include(x => x.CartItems)
                        .ThenInclude(x => x.MenuItem)
                        .FirstOrDefault(u => u.UserId == userId);
                }
                if(shoppingCart == null)
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return BadRequest(_response);
                }
                if(shoppingCart.CartItems != null && shoppingCart.CartItems.Count() > 0)
                {
                    shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);
                }
                _response.Result = shoppingCart;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }catch (Exception ex) 
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;

        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateItemInCart(string userId, int menuItemId, int updateQuantityBy)
        {
            ShoppingCart shoppingCart = _context.shoppingCarts.Include(u => u.CartItems).FirstOrDefault(u => u.UserId == userId);
            MenuItem menuItem = _context.MenuItems.FirstOrDefault(u => u.Id == menuItemId);
            if (menuItem == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            if (shoppingCart == null && updateQuantityBy>0) 
            {
                ShoppingCart newCart = new()
                {
                    UserId = userId,
                };
                _context.shoppingCarts.Add(newCart);
                _context.SaveChanges();

                CartItem newCartItem = new()
                {
                    MenuItemId = menuItemId,
                    Quantity = updateQuantityBy,
                    ShoppingCartId = newCart.Id,
                    MenuItem = null

                };
                _context.cartItems.Add(newCartItem);
                _context.SaveChanges();
            } else
            {
                CartItem cartItemDb = _context.cartItems.FirstOrDefault(u => u.MenuItemId == menuItemId); 
                if(cartItemDb == null)
                {
                    CartItem newCartItem = new()
                    {
                        MenuItemId = menuItemId,
                        Quantity = updateQuantityBy,
                        ShoppingCartId = shoppingCart.Id,
                        MenuItem = null

                    };
                    _context.cartItems.Add(newCartItem);
                    _context.SaveChanges();
                }
                else
                {
                    int newQuantity = cartItemDb.Quantity + updateQuantityBy;
                    if(updateQuantityBy == 0|| newQuantity <= 0)
                    {
                        _context.cartItems.Remove(cartItemDb);
                        if(shoppingCart.CartItems.Count() == 1)
                        {
                            _context.shoppingCarts.Remove(shoppingCart);
                        }
                        _context.SaveChanges();

                    } else
                    {
                        cartItemDb.Quantity = newQuantity;
                        _context.SaveChanges();
                    }

                }
            }
            return _response;
        }
    }
}
