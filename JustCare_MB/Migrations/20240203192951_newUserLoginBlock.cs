using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JustCare_MB.Migrations
{
    /// <inheritdoc />
    public partial class newUserLoginBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentBooked_Appointment_AppointmentId",
                table: "AppointmentBooked");

            migrationBuilder.AddColumn<DateTime>(
                name: "LoginBlock",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LoginCount",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentBooked_Appointment_AppointmentId",
                table: "AppointmentBooked",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentBooked_Appointment_AppointmentId",
                table: "AppointmentBooked");

            migrationBuilder.DropColumn(
                name: "LoginBlock",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LoginCount",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentBooked_Appointment_AppointmentId",
                table: "AppointmentBooked",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id");
        }
    }
}
