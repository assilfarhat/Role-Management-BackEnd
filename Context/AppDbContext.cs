using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Role_Management_BackEnd.Models;
using System.Reflection.Metadata;

namespace Role_Management_.NET.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Permissions> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Products>().ToTable("Products");
            modelBuilder.Entity<Roles>().ToTable("Roles");

           /* modelBuilder.Entity<User>()
           .HasOne(e => e.Role)
           .WithMany(e => e.Users)
           .HasForeignKey(e => e.RoleId)
           .IsRequired();*/

            modelBuilder.Entity<Roles>()
             .HasMany(r => r.Permissions)
             .WithMany(p => p.Roles)
             .UsingEntity<RolePermission>(
                 j => j.HasOne(rp => rp.Permission).WithMany().HasForeignKey("PermissionId"),
                  j => j.HasOne(rp => rp.Role).WithMany().HasForeignKey("RoleId")
      );
        }
    }
}
