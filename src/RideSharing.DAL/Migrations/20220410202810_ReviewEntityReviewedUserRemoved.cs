using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RideSharing.DAL.Migrations
{
    public partial class ReviewEntityReviewedUserRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationEntities_UserEntities_ReservingUserId",
                table: "ReservationEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewEntities_RideEntities_RideId",
                table: "ReviewEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewEntities_UserEntities_AuthorUserId",
                table: "ReviewEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewEntities_UserEntities_ReviewedUserId",
                table: "ReviewEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_RideEntities_VehicleEntities_VehicleId",
                table: "RideEntities");

            migrationBuilder.DropIndex(
                name: "IX_ReviewEntities_ReviewedUserId",
                table: "ReviewEntities");

            migrationBuilder.DropColumn(
                name: "ReviewedUserId",
                table: "ReviewEntities");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehicleId",
                table: "RideEntities",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "RideId",
                table: "ReviewEntities",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationEntities_UserEntities_ReservingUserId",
                table: "ReservationEntities",
                column: "ReservingUserId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewEntities_RideEntities_RideId",
                table: "ReviewEntities",
                column: "RideId",
                principalTable: "RideEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewEntities_UserEntities_AuthorUserId",
                table: "ReviewEntities",
                column: "AuthorUserId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RideEntities_VehicleEntities_VehicleId",
                table: "RideEntities",
                column: "VehicleId",
                principalTable: "VehicleEntities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationEntities_UserEntities_ReservingUserId",
                table: "ReservationEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewEntities_RideEntities_RideId",
                table: "ReviewEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewEntities_UserEntities_AuthorUserId",
                table: "ReviewEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_RideEntities_VehicleEntities_VehicleId",
                table: "RideEntities");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehicleId",
                table: "RideEntities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RideId",
                table: "ReviewEntities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewedUserId",
                table: "ReviewEntities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ReviewEntities_ReviewedUserId",
                table: "ReviewEntities",
                column: "ReviewedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationEntities_UserEntities_ReservingUserId",
                table: "ReservationEntities",
                column: "ReservingUserId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewEntities_RideEntities_RideId",
                table: "ReviewEntities",
                column: "RideId",
                principalTable: "RideEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewEntities_UserEntities_AuthorUserId",
                table: "ReviewEntities",
                column: "AuthorUserId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewEntities_UserEntities_ReviewedUserId",
                table: "ReviewEntities",
                column: "ReviewedUserId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RideEntities_VehicleEntities_VehicleId",
                table: "RideEntities",
                column: "VehicleId",
                principalTable: "VehicleEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
