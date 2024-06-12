using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedMango_api.Migrations
{
    /// <inheritdoc />
    public partial class shoppingcartupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientSecret",
                table: "shoppingCarts");

            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "shoppingCarts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientSecret",
                table: "shoppingCarts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "shoppingCarts",
                type: "text",
                nullable: true);
        }
    }
}
