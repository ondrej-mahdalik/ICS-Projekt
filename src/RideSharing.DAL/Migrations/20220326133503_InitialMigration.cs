using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RideSharing.DAL.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Seats = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleEntities_UserEntities_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RideEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SharedSeats = table.Column<int>(type: "int", nullable: false),
                    FromLatitude = table.Column<double>(type: "float", nullable: false),
                    FromLongitude = table.Column<double>(type: "float", nullable: false),
                    Distance = table.Column<int>(type: "int", nullable: false),
                    ToName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToLatitude = table.Column<double>(type: "float", nullable: false),
                    ToLongitude = table.Column<double>(type: "float", nullable: false),
                    Departure = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Arrival = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideEntities_VehicleEntities_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "VehicleEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservingUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RideId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Seats = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationEntities_RideEntities_RideId",
                        column: x => x.RideId,
                        principalTable: "RideEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationEntities_UserEntities_ReservingUserId",
                        column: x => x.ReservingUserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReviewEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RideId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewEntities_RideEntities_RideId",
                        column: x => x.RideId,
                        principalTable: "RideEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewEntities_UserEntities_AuthorUserId",
                        column: x => x.AuthorUserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewEntities_UserEntities_ReviewedUserId",
                        column: x => x.ReviewedUserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationEntities_ReservingUserId",
                table: "ReservationEntities",
                column: "ReservingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationEntities_RideId",
                table: "ReservationEntities",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewEntities_AuthorUserId",
                table: "ReviewEntities",
                column: "AuthorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewEntities_ReviewedUserId",
                table: "ReviewEntities",
                column: "ReviewedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewEntities_RideId",
                table: "ReviewEntities",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_RideEntities_VehicleId",
                table: "RideEntities",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleEntities_OwnerId",
                table: "VehicleEntities",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationEntities");

            migrationBuilder.DropTable(
                name: "ReviewEntities");

            migrationBuilder.DropTable(
                name: "RideEntities");

            migrationBuilder.DropTable(
                name: "VehicleEntities");

            migrationBuilder.DropTable(
                name: "UserEntities");
        }
    }
}
