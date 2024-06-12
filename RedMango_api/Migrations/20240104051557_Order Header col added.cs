using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedMango_api.Migrations
{
    /// <inheritdoc />
    public partial class OrderHeadercoladded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OrderTotal",
                table: "orderHeaders",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderTotal",
                table: "orderHeaders");
        }
    }
}
