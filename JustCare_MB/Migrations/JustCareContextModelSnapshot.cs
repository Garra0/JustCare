﻿// <auto-generated />
using System;
using JustCare_MB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JustCare_MB.Migrations
{
    [DbContext(typeof(JustCareContext))]
    partial class JustCareContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JustCare_MB.Models.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DentistUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DentistUserId");

                    b.ToTable("Appointment", (string)null);
                });

            modelBuilder.Entity("JustCare_MB.Models.AppointmentInfo.AppointmentBooked", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AppointmentId")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("PatientUserId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId")
                        .IsUnique();

                    b.HasIndex("PatientUserId");

                    b.ToTable("AppointmentBooked", (string)null);
                });

            modelBuilder.Entity("JustCare_MB.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArabicName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("EnglishName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("JustCare_MB.Models.Categorys.MedicalHistoryStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MedicalHistoryId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedicalHistoryId");

                    b.HasIndex("UserId");

                    b.ToTable("MedicalHistoryStatus", (string)null);
                });

            modelBuilder.Entity("JustCare_MB.Models.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArabicType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("EnglishType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Gender", (string)null);
                });

            modelBuilder.Entity("JustCare_MB.Models.Lookup.MedicalHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArabicDisease")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("EnglishDisease")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.ToTable("MedicalHistories");
                });

            modelBuilder.Entity("JustCare_MB.Models.Lookup.UserType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArabicType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("EnglishType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("UserType", (string)null);
                });

            modelBuilder.Entity("JustCare_MB.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("GenderId")
                        .HasColumnType("int");

                    b.Property<int>("NationalId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("PhoneNumber")
                        .HasColumnType("int");

                    b.Property<int>("UserTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GenderId");

                    b.HasIndex("UserTypeId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("JustCare_MB.Models.Appointment", b =>
                {
                    b.HasOne("JustCare_MB.Models.Category", "Category")
                        .WithMany("Appointments")
                        .HasForeignKey("CategoryId")
                        .IsRequired();

                    b.HasOne("JustCare_MB.Models.User", "DentistUser")
                        .WithMany("Appointments")
                        .HasForeignKey("DentistUserId")
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("DentistUser");
                });

            modelBuilder.Entity("JustCare_MB.Models.AppointmentInfo.AppointmentBooked", b =>
                {
                    b.HasOne("JustCare_MB.Models.Appointment", "Appointment")
                        .WithOne("AppointmentBooked")
                        .HasForeignKey("JustCare_MB.Models.AppointmentInfo.AppointmentBooked", "AppointmentId")
                        .IsRequired();

                    b.HasOne("JustCare_MB.Models.User", "PatientUser")
                        .WithMany("AppointmentBookeds")
                        .HasForeignKey("PatientUserId")
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("PatientUser");
                });

            modelBuilder.Entity("JustCare_MB.Models.Categorys.MedicalHistoryStatus", b =>
                {
                    b.HasOne("JustCare_MB.Models.Lookup.MedicalHistory", "MedicalHistory")
                        .WithMany("MedicalHistoryStatuses")
                        .HasForeignKey("MedicalHistoryId")
                        .IsRequired();

                    b.HasOne("JustCare_MB.Models.User", "User")
                        .WithMany("MedicalHistoryStatuses")
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("MedicalHistory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("JustCare_MB.Models.User", b =>
                {
                    b.HasOne("JustCare_MB.Models.Gender", "Gender")
                        .WithMany("Users")
                        .HasForeignKey("GenderId")
                        .IsRequired();

                    b.HasOne("JustCare_MB.Models.Lookup.UserType", "UserType")
                        .WithMany("Users")
                        .HasForeignKey("UserTypeId")
                        .IsRequired();

                    b.Navigation("Gender");

                    b.Navigation("UserType");
                });

            modelBuilder.Entity("JustCare_MB.Models.Appointment", b =>
                {
                    b.Navigation("AppointmentBooked")
                        .IsRequired();
                });

            modelBuilder.Entity("JustCare_MB.Models.Category", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("JustCare_MB.Models.Gender", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("JustCare_MB.Models.Lookup.MedicalHistory", b =>
                {
                    b.Navigation("MedicalHistoryStatuses");
                });

            modelBuilder.Entity("JustCare_MB.Models.Lookup.UserType", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("JustCare_MB.Models.User", b =>
                {
                    b.Navigation("AppointmentBookeds");

                    b.Navigation("Appointments");

                    b.Navigation("MedicalHistoryStatuses");
                });
#pragma warning restore 612, 618
        }
    }
}
