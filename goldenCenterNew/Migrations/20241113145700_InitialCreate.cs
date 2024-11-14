using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace goldenCenterNew.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CT_DeviceTypes",
                columns: table => new
                {
                    PKDeviceTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CyclesLimit = table.Column<int>(type: "int", nullable: false),
                    WeeklyCyclesLimit = table.Column<int>(type: "int", nullable: false),
                    AlertThreshold = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_DeviceTypes", x => x.PKDeviceTypeID);
                });

            migrationBuilder.CreateTable(
                name: "CT_Roles",
                columns: table => new
                {
                    PKRoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_Roles", x => x.PKRoleID);
                });

            migrationBuilder.CreateTable(
                name: "SC_Users",
                columns: table => new
                {
                    PKUserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NTAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SC_Users", x => x.PKUserID);
                });

            migrationBuilder.CreateTable(
                name: "CT_Devices",
                columns: table => new
                {
                    PKDeviceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FKTypeID = table.Column<int>(type: "int", nullable: false),
                    Cycles = table.Column<int>(type: "int", nullable: false),
                    WeeklyCycles = table.Column<int>(type: "int", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_Devices", x => x.PKDeviceID);
                    table.ForeignKey(
                        name: "FK_CT_Devices_CT_DeviceTypes_FKTypeID",
                        column: x => x.FKTypeID,
                        principalTable: "CT_DeviceTypes",
                        principalColumn: "PKDeviceTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CT_UsersRoles",
                columns: table => new
                {
                    PKUserRoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FKUserID = table.Column<int>(type: "int", nullable: false),
                    FKRoleID = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_UsersRoles", x => x.PKUserRoleID);
                    table.ForeignKey(
                        name: "FK_CT_UsersRoles_CT_Roles_FKRoleID",
                        column: x => x.FKRoleID,
                        principalTable: "CT_Roles",
                        principalColumn: "PKRoleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CT_UsersRoles_SC_Users_FKUserID",
                        column: x => x.FKUserID,
                        principalTable: "SC_Users",
                        principalColumn: "PKUserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CR_Alerts",
                columns: table => new
                {
                    PKAlertID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FKDeviceID = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR_Alerts", x => x.PKAlertID);
                    table.ForeignKey(
                        name: "FK_CR_Alerts_CT_Devices_FKDeviceID",
                        column: x => x.FKDeviceID,
                        principalTable: "CT_Devices",
                        principalColumn: "PKDeviceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CR_CyclesHistories",
                columns: table => new
                {
                    PKCycleHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FKDeviceID = table.Column<int>(type: "int", nullable: false),
                    Week = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR_CyclesHistories", x => x.PKCycleHistoryID);
                    table.ForeignKey(
                        name: "FK_CR_CyclesHistories_CT_Devices_FKDeviceID",
                        column: x => x.FKDeviceID,
                        principalTable: "CT_Devices",
                        principalColumn: "PKDeviceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CR_Alerts_FKDeviceID",
                table: "CR_Alerts",
                column: "FKDeviceID");

            migrationBuilder.CreateIndex(
                name: "IX_CR_CyclesHistories_FKDeviceID",
                table: "CR_CyclesHistories",
                column: "FKDeviceID");

            migrationBuilder.CreateIndex(
                name: "IX_CT_Devices_FKTypeID",
                table: "CT_Devices",
                column: "FKTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CT_UsersRoles_FKRoleID",
                table: "CT_UsersRoles",
                column: "FKRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_CT_UsersRoles_FKUserID",
                table: "CT_UsersRoles",
                column: "FKUserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CR_Alerts");

            migrationBuilder.DropTable(
                name: "CR_CyclesHistories");

            migrationBuilder.DropTable(
                name: "CT_UsersRoles");

            migrationBuilder.DropTable(
                name: "CT_Devices");

            migrationBuilder.DropTable(
                name: "CT_Roles");

            migrationBuilder.DropTable(
                name: "SC_Users");

            migrationBuilder.DropTable(
                name: "CT_DeviceTypes");
        }
    }
}
