using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JustCare_MB.Migrations
{
    /// <inheritdoc />
    public partial class MergeUsersImageTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DentistAppointmentImage");

            migrationBuilder.DropTable(
                name: "PatientAppointmentImage");

            migrationBuilder.CreateTable(
                name: "UserAppointmentImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    AppointmentBookedId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAppointmentImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAppointmentImage_AppointmentBooked_AppointmentBookedId",
                        column: x => x.AppointmentBookedId,
                        principalTable: "AppointmentBooked",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAppointmentImage_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAppointmentImage_AppointmentBookedId",
                table: "UserAppointmentImage",
                column: "AppointmentBookedId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAppointmentImage_AppointmentId",
                table: "UserAppointmentImage",
                column: "AppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAppointmentImage");

            migrationBuilder.CreateTable(
                name: "DentistAppointmentImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentistAppointmentImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DentistAppointmentImage_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PatientAppointmentImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentBookedId = table.Column<int>(type: "int", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "IX_DentistAppointmentImage_AppointmentId",
                table: "DentistAppointmentImage",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAppointmentImage_AppointmentBookedId",
                table: "PatientAppointmentImage",
                column: "AppointmentBookedId");
        }
    }
}
