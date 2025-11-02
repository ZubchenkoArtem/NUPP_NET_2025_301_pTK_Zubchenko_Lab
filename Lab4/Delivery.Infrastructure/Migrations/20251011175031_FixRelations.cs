using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrders_Vehicles_AssignedVehicleId",
                table: "DeliveryOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrders_Vehicles_AssignedVehicleId",
                table: "DeliveryOrders",
                column: "AssignedVehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrders_Vehicles_AssignedVehicleId",
                table: "DeliveryOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrders_Vehicles_AssignedVehicleId",
                table: "DeliveryOrders",
                column: "AssignedVehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
