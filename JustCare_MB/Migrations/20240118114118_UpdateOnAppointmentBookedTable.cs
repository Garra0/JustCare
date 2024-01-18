using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JustCare_MB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOnAppointmentBookedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "AppointmentBooked");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "AppointmentBooked",
                newName: "PatientDescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatientDescription",
                table: "AppointmentBooked",
                newName: "Note");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "AppointmentBooked",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
