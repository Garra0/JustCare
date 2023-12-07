using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JustCare_MB.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ArabicName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gender",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnglishType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ArabicType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnglishDisease = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ArabicDisease = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnglishType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ArabicType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Gender_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Gender",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_UserType_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DentistUserId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointment_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointment_User_DentistUserId",
                        column: x => x.DentistUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MedicalHistoryStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MedicalHistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistoryStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalHistoryStatus_MedicalHistories_MedicalHistoryId",
                        column: x => x.MedicalHistoryId,
                        principalTable: "MedicalHistories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalHistoryStatus_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppointmentBooked",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    PatientUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentBooked", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentBooked_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppointmentBooked_User_PatientUserId",
                        column: x => x.PatientUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_CategoryId",
                table: "Appointment",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_DentistUserId",
                table: "Appointment",
                column: "DentistUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentBooked_AppointmentId",
                table: "AppointmentBooked",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentBooked_PatientUserId",
                table: "AppointmentBooked",
                column: "PatientUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistoryStatus_MedicalHistoryId",
                table: "MedicalHistoryStatus",
                column: "MedicalHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistoryStatus_UserId",
                table: "MedicalHistoryStatus",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_GenderId",
                table: "User",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserTypeId",
                table: "User",
                column: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentBooked");

            migrationBuilder.DropTable(
                name: "MedicalHistoryStatus");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "MedicalHistories");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Gender");

            migrationBuilder.DropTable(
                name: "UserType");
        }
    }
}
