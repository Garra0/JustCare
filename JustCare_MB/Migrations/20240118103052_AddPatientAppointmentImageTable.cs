using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JustCare_MB.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientAppointmentImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientAppointmentImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentBookedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAppointmentImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAppointmentImage_AppointmentBooked_AppointmentBookedId",
                        column: x => x.AppointmentBookedId,
                        principalTable: "AppointmentBooked",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientAppointmentImage_AppointmentBookedId",
                table: "PatientAppointmentImage",
                column: "AppointmentBookedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientAppointmentImage");
        }
    }
}
