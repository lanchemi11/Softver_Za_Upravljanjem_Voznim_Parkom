using FleetManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Data
{
    public class AppDbContext : DbContext
    {
        // Konstruktor za DI i migracije
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Default konstruktor
        public AppDbContext() : base(new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=fleet.db").Options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Vozac> Vozaci { get; set; }
        public DbSet<Administrator> Administratori { get; set; }
        public DbSet<Menadzer> Menadzeri { get; set; }

        public DbSet<Vozilo> Vozila { get; set; }
        public DbSet<Servis> Servisi { get; set; }
        public DbSet<Izvestaj> Izvestaji { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Vozac>().ToTable("Vozaci");
            modelBuilder.Entity<Administrator>().ToTable("Administratori");
            modelBuilder.Entity<Menadzer>().ToTable("Menadzeri");

            // Relacija: Vozilo → Servis (1 → *)
            modelBuilder.Entity<Servis>()
                .HasOne(s => s.Vozilo)
                .WithMany(v => v.Servisi)
                .HasForeignKey(s => s.VoziloId);

            // Relacija: Vozac → Vozilo (1 → 0..*)
            modelBuilder.Entity<Vozilo>()
                .HasOne(v => v.Vozac)
                .WithMany(vo => vo.Vozila)
                .HasForeignKey(v => v.VozacId)
                .IsRequired(false);

            // Relacija: Izvestaj → Vozilo (0..* agregacija)
            modelBuilder.Entity<Izvestaj>()
                .HasMany(i => i.Vozila)
                .WithMany();
        }
    }

}
