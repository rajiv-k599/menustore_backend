using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedMango_api.Data;
using RedMango_api.Models;
using RedMango_api.Models.Dto;
using RedMango_api.Services;
using RedMango_api.sd;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RedMango_api.Controllers
{
    [Route("api/MenuItem")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IBlobService _blobService;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private ApiResponse _response;
       
        public MenuItemController(AppDbContext context,IBlobService blobService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _blobService = blobService;
            _response = new ApiResponse();
            WebHostEnvironment = webHostEnvironment;

        }

        [HttpGet]
        
        public async Task<IActionResult> GetMenuItems()
        {
            _response.Result = _context.MenuItems;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }


        [HttpGet("{id:int}",Name = "GetMenuItem")]
        
        public async Task<IActionResult> GetMenuItem(int id)
        {
            if (id == 0)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            MenuItem menuItem = _context.MenuItems.FirstOrDefault(x => x.Id == id);
            if (menuItem == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.Result = menuItem;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm]MenuItemCreateDto menuItemCreateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(menuItemCreateDto.File == null || menuItemCreateDto.File.Length == 0) 
                    {
                        _response.IsSuccess = false;
                        return BadRequest();
                    }
                    string fileName = $"{ Guid.NewGuid()}{Path.GetExtension(menuItemCreateDto.File.FileName) }";
                    MenuItem menuItemCreate = new()
                    {
                        Name = menuItemCreateDto.Name,
                        Price = menuItemCreateDto.Price,
                        Categories = menuItemCreateDto.Categories,
                        SpecialTag = menuItemCreateDto.SpecialTag,
                        Description = menuItemCreateDto.Description,
                        Image = "https://localhost:7250/images/"+upload(menuItemCreateDto.File,fileName),
                        // Image = await _blobService.UploadBlob(fileName, SD.sd_storage_container, menuItemCreateDto.File),
                    };
                    _context.MenuItems.Add(menuItemCreate);
                    _context.SaveChanges();
                    _response.Result = menuItemCreate;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetMenuItem", new { id = menuItemCreate.Id }, _response);
                     
                } else { _response.IsSuccess = false; }

            } catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
             
            }
            return _response; 
        }
        private string upload(IFormFile image,string filename)
        {
           
            if (image != null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "images");
               
                string filePath = Path.Combine(uploadDir, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            return filename;
        }
        private async Task<string> DeleteAsync(string filePath)
        {
            string filename = filePath.Split('/').Last();
            string delDir = Path.Combine(WebHostEnvironment.WebRootPath, "images", filename);

            FileInfo fileInfo = new FileInfo(delDir);

            if (fileInfo.Exists)
            {
                await Task.Run(() => System.IO.File.Delete(delDir));
                fileInfo.Delete();
            }

            return filename;
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<ActionResult<ApiResponse>> UpdateMenuItem(int id,[FromForm] MenuItemUpdateDto menuItemUpdateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (menuItemUpdateDto == null || id != menuItemUpdateDto.Id)
                    {
                        _response.IsSuccess = false;
                        return BadRequest();
                    }
                    MenuItem menuItemFromDb = await _context.MenuItems.FindAsync(id);
                    if (menuItemFromDb == null)
                    {
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    menuItemFromDb.Name = menuItemUpdateDto.Name;
                    menuItemFromDb.Price = menuItemUpdateDto.Price;
                    menuItemFromDb.Categories = menuItemUpdateDto.Categories;
                    menuItemFromDb.SpecialTag = menuItemUpdateDto.SpecialTag;
                    menuItemFromDb.Description = menuItemUpdateDto.Description;

                    if (menuItemUpdateDto.File != null && menuItemUpdateDto.File.Length > 0)
                    {
                        
                            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(menuItemUpdateDto.File.FileName)}";
                            await DeleteAsync(menuItemFromDb.Image);
                           // await _blobService.DeleteBlob(menuItemFromDb.Image.Split('/').Last(), SD.sd_storage_container);
                            menuItemFromDb.Image = "https://localhost:7250/images/" + upload(menuItemUpdateDto.File, fileName);

                    }



                        _context.MenuItems.Update(menuItemFromDb);
                        _context.SaveChanges();

                        _response.StatusCode = HttpStatusCode.NoContent;
                        return Ok(_response);

                    }
                    else { _response.IsSuccess = false; }

                
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<ActionResult<ApiResponse>> DeleteMenuItem(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                MenuItem menuItemFromDb = await _context.MenuItems.FindAsync(id);
                if (menuItemFromDb == null)
                {
                    return BadRequest();
                }

                await _blobService.DeleteBlob(menuItemFromDb.Image.Split('/').Last(), SD.sd_storage_container);
                int miliseconds = 2000;
                Thread.Sleep(miliseconds);
                _context.MenuItems.Remove(menuItemFromDb);
                _context.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
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
