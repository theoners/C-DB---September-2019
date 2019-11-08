using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<PatientMedicament> PatientsMedicaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Visitation>(entity =>
            {
               
                entity
                        .HasOne(v => v.Patient)
                        .WithMany(p => p.Visitations)
                        .HasForeignKey(v => v.PatientId)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Visitations_Patients");
            });

            modelBuilder.Entity<Diagnose>(entity =>
            {
               entity.HasOne(d => d.Patient)
                      .WithMany(p => p.Diagnoses)
                      .HasForeignKey(d => d.PatientId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Diagnosess_Patients");
            });

          
            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(pm => new
                {
                    pm.PatientId,
                    pm.MedicamentId
                });

               

                entity.HasOne(pm => pm.Patient)
                      .WithMany(p => p.Prescriptions)
                      .HasForeignKey(pm => pm.PatientId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_PatientsMedicaments_Patients");

                entity.HasOne(pm => pm.Medicament)
                      .WithMany(m => m.Prescriptions)
                      .HasForeignKey(pm => pm.MedicamentId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_PatientsMedicaments_Medicaments");
            });
        }
    }
}
