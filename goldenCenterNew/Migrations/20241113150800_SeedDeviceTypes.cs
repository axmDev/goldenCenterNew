using goldenCenterNew.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace goldenCenterNew.Migrations
{
    /// <inheritdoc />
    public partial class SeedDeviceTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CT_DeviceTypes",
                columns: new[]{ "PKDeviceTypeID", "Type", "CyclesLimit",
                    "WeeklyCyclesLimit", "AlertThreshold", "Description",
                    "LastUpdated", "Available" },
                values: new object[,]
                {
                    {1, 1, 1000, 100, 80.0, "Battery Device", DateTime.Now, true },
                    {2, 2, 1000, 100, 80.0, "Rectifier Device", DateTime.Now, true },
                    {3, 3, 1000, 100, 80.0, "Umihebi Devie", DateTime.Now, true }
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CT_DeviceTypes",
                keyColumn: "PKDeviceTypeID",
                keyValues: new object[] { 1, 2, 3 });
        }
    }
}
