using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniERP_Suministros.Server.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureDeleteBehaviors_Order_OrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrderDetails_AppProducts_ProductId",
                table: "AppOrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrderDetails_AppProducts_ProductId",
                table: "AppOrderDetails",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrderDetails_AppProducts_ProductId",
                table: "AppOrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrderDetails_AppProducts_ProductId",
                table: "AppOrderDetails",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
