using Microsoft.EntityFrameworkCore;
using PartFinderCore.Classes;

namespace PartFinderCore.Models;

public class SiteContext : DbContext
{
    public DbSet<Footprint> Footprint { get; set; } = null!;
    public DbSet<FootprintCategory> FootprintCategory { get; set; } = null!;
    public DbSet<Manufacturer> Manufacturer { get; set; } = null!;
    public DbSet<PartAttachment> PartAttachment { get; set; } = null!;
    public DbSet<PartCategory> PartCategory { get; set; } = null!;
    public DbSet<PartParameter> PartParameter { get; set; } = null!;
    public DbSet<Parts> Parts { get; set; } = null!;
    public DbSet<PartStockLevelHistory> PartStockLevelHistory { get; set; } = null!;
    public DbSet<PartSuppliers> PartSuppliers { get; set; } = null!;
    public DbSet<StorageLocation> StorageLocation { get; set; } = null!;
    public DbSet<Users> Users { get; set; } = null!;

            
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var databasePath = GlobalData.SiteData.DataBasePath;
        var connectionString = $"Data Source={databasePath}";
        optionsBuilder.UseSqlite(connectionString);
        base.OnConfiguring(optionsBuilder);
           
    }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Parts>()
            .HasOne(p => p.StorageLocation)
            .WithMany(s => s.Parts)
            .HasForeignKey(p => p.StorageLocationID);


        modelBuilder.Entity<Parts>()
            .HasOne(p => p.Manufacturer)
            .WithMany(m => m.Parts)
            .HasForeignKey(p => p.PartManID);

        modelBuilder.Entity<Parts>()
            .HasOne(p => p.Footprint)
            .WithMany(f => f.Parts)
            .HasForeignKey(p => p.PartFootprintID);

        modelBuilder.Entity<Parts>()
            .HasOne(p => p.PartCategory)
            .WithMany(pc => pc.Parts)
            .HasForeignKey(p => p.PartCategoryID);

        modelBuilder.Entity<Footprint>()
            .HasOne(f => f.FootprintCategoryNavigation)
            .WithMany(fc => fc.Footprints)
            .HasForeignKey(f => f.FootprintCategory)
            .HasPrincipalKey(fc => fc.FCPkey);

        base.OnModelCreating(modelBuilder);
    }
}