using NIF_Builder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NIF_Builder.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<ProjectEquipment> ProjectEquipments { get; set; }
    }
}
