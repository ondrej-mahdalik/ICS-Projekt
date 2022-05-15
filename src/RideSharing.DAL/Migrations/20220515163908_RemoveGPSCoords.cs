using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RideSharing.DAL.Migrations
{
    public partial class RemoveGPSCoords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromLatitude",
                table: "RideEntities");

            migrationBuilder.DropColumn(
                name: "FromLongitude",
                table: "RideEntities");

            migrationBuilder.DropColumn(
                name: "ToLatitude",
                table: "RideEntities");

            migrationBuilder.DropColumn(
                name: "ToLongitude",
                table: "RideEntities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FromLatitude",
                table: "RideEntities",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FromLongitude",
                table: "RideEntities",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ToLatitude",
                table: "RideEntities",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ToLongitude",
                table: "RideEntities",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
