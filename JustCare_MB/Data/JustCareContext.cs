using Microsoft.EntityFrameworkCore;
using JustCare_MB.Models;

namespace JustCare_MB.Data
{
    public class JustCareContext : DbContext
    {
        public JustCareContext()
        {
        }

        public JustCareContext(DbContextOptions<JustCareContext> options)
        : base(options) { }


        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentBooked> AppointmentBookeds { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<MedicalHistoryStatus> MedicalHistoryStatuses { get; set; }

        public DbSet<MedicalHistory> MedicalHistories { get; set; }


       // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       //=> optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=JustCare3;Trusted_Connection=True;");
       



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.ToTable("User");

                entity.HasOne(u => u.Gender)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(u => u.UserType)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasMany(u => u.Appointments)
                .WithOne(u => u.DentistUser)
                .HasForeignKey(u => u.DentistUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasMany(u => u.AppointmentBookeds)
                .WithOne(u => u.PatientUser)
                 .HasForeignKey(u => u.PatientUserId)
                 .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasMany(u => u.MedicalHistoryStatuses)
                  .WithOne(u => u.User)
                 .HasForeignKey(u => u.UserId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<UserType>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.ToTable("UserType");
            });
            
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.ToTable("Category");
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.ToTable("Appointment");

                entity.HasOne(u => u.Category)
                .WithMany(u => u.Appointments)
                .HasForeignKey(u => u.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AppointmentBooked>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.ToTable("AppointmentBooked");

                entity.HasOne(u => u.Appointment)
               .WithOne(u => u.AppointmentBooked)
                // .HasForeignKey(u => u.AppointmentId)
               .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.ToTable("Gender");
            });

            modelBuilder.Entity<MedicalHistoryStatus>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.ToTable("MedicalHistoryStatus");

                entity.HasOne(u => u.MedicalHistory)
               .WithMany(u => u.MedicalHistoryStatuses)
               .HasForeignKey(u => u.MedicalHistoryId)
               .OnDelete(DeleteBehavior.ClientSetNull);
            });
               

            //modelBuilder.Entity<AppointmentBooked>()
            //   .HasOne(u => u.Appointment)
            //   .WithOne(u => u.AppointmentBooked)
            //   // .HasForeignKey(u => u.AppointmentId)
            //   .OnDelete(DeleteBehavior.Restrict);


            //modelBuilder.Entity<Appointment>()
            //    .HasOne(u => u.Category)
            //    .WithMany(u => u.Appointments)
            //    .HasForeignKey(u => u.CategoryId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<MedicalHistoryStatus>()
            //   .HasOne(u => u.MedicalHistory)
            //   .WithMany(u => u.MedicalHistoryStatuses)
            //   .HasForeignKey(u => u.MedicalHistoryId)
            //   .OnDelete(DeleteBehavior.Restrict);



            // one of these create the same relation
            // 1-
            //modelBuilder.Entity<User>()
            //    .HasMany(u=>u.Appointments)
            //    .WithOne(u=>u.DentistUser)
            //    .HasForeignKey(u => u.DentistUserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // 2-
            //modelBuilder.Entity<Appointment>()
            //    .HasOne(u => u.DentistUser)
            //    .WithMany(u => u.Appointments)
            //    .HasForeignKey(u => u.DentistUserId)
            //    .OnDelete(DeleteBehavior.Restrict);







            //OnModelCreatingPartial(modelBuilder);
        }
        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    } 
}
