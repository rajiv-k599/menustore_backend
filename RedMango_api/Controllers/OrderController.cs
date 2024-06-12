using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedMango_api.Data;
using RedMango_api.Models;
using RedMango_api.Models.Dto;
using RedMango_api.sd;
using RedMango_api.Services;
using System.Net;
using System.Text.Json;

namespace RedMango_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
     
        private ApiResponse _response;

        public OrderController(AppDbContext context)
        {
            _context = context;
           
            _response = new ApiResponse();

        }

        [HttpGet]
       
        public async Task<ActionResult<ApiResponse>> GetOrders(string? userId, string searchString, string status, int pageNumber=1, int pageSize=5)
        {
            try
            {
                IEnumerable<OrderHeader> orderHeaders = _context.orderHeaders.Include(u => u.OrderDetails)
                    .ThenInclude(u => u.MenuItem).OrderByDescending(u => u.OrderHeaderId);
                if(!string.IsNullOrEmpty(userId) )
                {
                    orderHeaders= orderHeaders.Where(u => u.ApplicationUserId == userId);
                } 
                if(!string.IsNullOrEmpty(searchString) )
                {
                    orderHeaders = orderHeaders.Where(u => u.PickupPhone.ToLower().Contains(searchString.ToLower()) ||
                    u.PickupEmail.ToLower().Contains(searchString.ToLower()) || u.PickupName.ToLower().Contains(searchString.ToLower()));
                }
                if(!string.IsNullOrEmpty(status) )
                {
                    orderHeaders = orderHeaders.Where(u=> u.Status.ToLower() == status.ToLower());
                }
                Pagination pagination = new()
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = orderHeaders.Count()
                };
                Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(pagination));
                _response.Result = orderHeaders.Skip((pageNumber - 1) * pageSize).Take(pageSize);
          
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            } catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetOrders(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var orderHeaders = _context.orderHeaders.Include(u => u.OrderDetails)
                    .ThenInclude(u => u.MenuItem)
                    .Where(u => u.OrderHeaderId == id);
                if (orderHeaders == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                
                    _response.Result = orderHeaders;

                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;

        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDto orderHeaderDto)
        {
            try
            {
                OrderHeader order = new()
                {
                    ApplicationUserId = orderHeaderDto.ApplicationUserId,
                    PickupEmail = orderHeaderDto.PickupEmail,
                    PickupName = orderHeaderDto.PickupName,
                    PickupPhone = orderHeaderDto.PickupPhone,
                    OrderTotal = orderHeaderDto.OrderTotal,
                    orderDate = DateTime.UtcNow,
                    StripePaymentIntentId = orderHeaderDto.StripePaymentIntentId,
                    TotalItems = orderHeaderDto.TotalItems,
                    Status = String.IsNullOrEmpty(orderHeaderDto.Status) ? SD.status_pending : orderHeaderDto.Status,

                };
                if(ModelState.IsValid )
                {
                    _context.orderHeaders.Add(order);
                    _context.SaveChanges();
                    foreach(var orderDetailDto in orderHeaderDto.OrderDetails)
                    {
                        OrderDetails orderDetails = new()
                        {
                            OrderHeaderId = order.OrderHeaderId,
                            ItemName = orderDetailDto.ItemName,
                            MenuItemId = orderDetailDto.MenuItemId,
                            Price = orderDetailDto.Price,
                            Quantity = orderDetailDto.Quantity,
                        };
                        _context.orderDetails.Add(orderDetails);
                    }
                    _context.SaveChanges();
                    _response.Result = order;
                    order.OrderDetails = null;
                    _response.StatusCode = HttpStatusCode.Created;
                    return Ok(_response);
                }

            } catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;

        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateHeaderOrder(int id,[FromBody] OrderHeaderUpdateDto orderHeaderUpdateDto)
        {
            try
            {
                if (orderHeaderUpdateDto == null || id != orderHeaderUpdateDto.OrderHeaderId)
                {
                    return BadRequest();
                }
                OrderHeader orderFromDb = _context.orderHeaders.FirstOrDefault(u => u.OrderHeaderId == id);
                if (orderFromDb == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();

                }
                if(!String.IsNullOrEmpty(orderHeaderUpdateDto.PickupName))
                {
                    orderFromDb.PickupName = orderHeaderUpdateDto.PickupName;
                }
                if (!String.IsNullOrEmpty(orderHeaderUpdateDto.PickupEmail))
                {
                    orderFromDb.PickupEmail = orderHeaderUpdateDto.PickupEmail;
                }

                if (!String.IsNullOrEmpty(orderHeaderUpdateDto.PickupPhone))
                {
                    orderFromDb.PickupPhone = orderHeaderUpdateDto.PickupPhone;
                }
                if (!String.IsNullOrEmpty(orderHeaderUpdateDto.StripePaymentIntentId))
                {
                    orderFromDb.StripePaymentIntentId = orderHeaderUpdateDto.StripePaymentIntentId;
                }
                if (!String.IsNullOrEmpty(orderHeaderUpdateDto.Status))
                {
                    orderFromDb.Status = orderHeaderUpdateDto.Status;
                }
                _context.SaveChanges();

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        }
}
