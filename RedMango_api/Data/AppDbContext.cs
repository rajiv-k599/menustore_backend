using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RedMango_api.Models;

namespace RedMango_api.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<CartItem> cartItems { get; set; }
        public DbSet<OrderHeader> orderHeaders { get; set; }
        public DbSet<OrderDetails> orderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MenuItem>().HasData(
               new MenuItem
               {
                   Id = 1,
                   Name = "Spring Roll",
                   Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                   Image = "https://redmahgoimages.blob.core.windows.net/redmango/spring roll.jpg",
                   Price = 7.99,
                   Categories = "Appetizer",
                   SpecialTag = ""
               }, new MenuItem
               {
                   Id = 2,
                   Name = "Idli",
                   Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                   Image = "https://redmahgoimages.blob.core.windows.net/redmango/idli.jpg",
                   Price = 8.99,
                   Categories = "Appetizer",
                   SpecialTag = ""
               }, new MenuItem
               {
                   Id = 3,
                   Name = "Panu Puri",
                   Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                   Image = "https://redmahgoimages.blob.core.windows.net/redmango/pani puri.jpg",
                   Price = 8.99,
                   Categories = "Appetizer",
                   SpecialTag = "Best Seller"
               }, new MenuItem
               {
                   Id = 4,
                   Name = "Hakka Noodles",
                   Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                   Image = "https://redmahgoimages.blob.core.windows.net/redmango/hakka noodles.jpg",
                   Price = 10.99,
                   Categories = "Entrée",
                   SpecialTag = ""
               }, new MenuItem
               {
                   Id = 5,
                   Name = "Malai Kofta",
                   Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                   Image = "https://redmahgoimages.blob.core.windows.net/redmango/malai kofta.jpg",
                   Price = 12.99,
                   Categories = "Entrée",
                   SpecialTag = "Top Rated"
               }, new MenuItem
               {
                   Id = 6,
                   Name = "Paneer Pizza",
                   Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                   Image = "https://redmahgoimages.blob.core.windows.net/redmango/paneer pizza.jpg",
                   Price = 11.99,
                   Categories = "Entrée",
                   SpecialTag = ""
               }, new MenuItem
               {
                   Id = 7,
                   Name = "Paneer Tikka",
                   Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                   Image = "https://redmahgoimages.blob.core.windows.net/redmango/paneer tikka.jpg",
                   Price = 13.99,
                   Categories = "Entrée",
                   SpecialTag = "Chef's Special"
               }, new MenuItem
               {
                   Id = 8,
                   Name = "Carrot Love",
                   Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                   Image = "https://redmahgoimages.blob.core.windows.net/redmango/carrot love.jpg",
                   Price = 4.99,
                   Categories = "Dessert",
                   SpecialTag = ""
               }, new MenuItem
               {
                   Id = 9,
                   Name = "Rasmalai",
                   Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                   Image = "https://redmahgoimages.blob.core.windows.net/redmango/rasmalai.jpg",
                   Price = 4.99,
                   Categories = "Dessert",
                   SpecialTag = "Chef's Special"
               }, new MenuItem
               {
                   Id = 10,
                   Name = "Sweet Rolls",
                   Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                   Image = "https://redmahgoimages.blob.core.windows.net/redmango/sweet rolls.jpg",
                   Price = 3.99,
                   Categories = "Dessert",
                   SpecialTag = "Top Rated"
               });
        }
    }
}
