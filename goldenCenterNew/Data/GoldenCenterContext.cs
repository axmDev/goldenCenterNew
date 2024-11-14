using goldenCenterNew.Models;
using Microsoft.EntityFrameworkCore;

namespace goldenCenterNew.Data
{
    public class GoldenCenterContext : DbContext
    {
        public GoldenCenterContext(DbContextOptions<GoldenCenterContext> options) : base(options) { }

        public DbSet<CR_Alerts> CR_Alerts { get; set; }
        public DbSet<CR_CyclesHistories> CR_CyclesHistories { get; set; }
        public DbSet<CT_DeviceTypes> CT_DeviceTypes { get; set; }
        public DbSet<CT_Devices> CT_Devices { get; set; }
        public DbSet<CT_Roles> CT_Roles { get; set; }
        public DbSet<CT_UsersRoles> CT_UsersRoles { get; set; }
        public DbSet<SC_Users> SC_Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CR_Alerts>().ToTable("CR_Alerts");
            modelBuilder.Entity<CR_CyclesHistories>().ToTable("CR_CyclesHistories");
            modelBuilder.Entity<CT_DeviceTypes>().ToTable("CT_DeviceTypes");
            modelBuilder.Entity<CT_Devices>().ToTable("CT_Devices");
            modelBuilder.Entity<CT_Roles>().ToTable("CT_Roles");
            modelBuilder.Entity<CT_UsersRoles>().ToTable("CT_UsersRoles");
            modelBuilder.Entity<SC_Users>().ToTable("SC_Users");
        }
    }

}
