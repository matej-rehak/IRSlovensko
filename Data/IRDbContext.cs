using IRSlovensko.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IRSlovensko.Data;

public class IRDbContextFactory : IDesignTimeDbContextFactory<IRDbContext>
{
    public IRDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<IRDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=IRSlovensko;Trusted_Connection=True;")
            .Options;
        return new IRDbContext(options);
    }
}

public class IRDbContext(DbContextOptions<IRDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=IRSlovensko;Trusted_Connection=True;");
    }

    public DbSet<Konanie> Konania { get; set; }
    public DbSet<Osoba> Osoby { get; set; }
    public DbSet<Spravca> Spravcovia { get; set; }
    public DbSet<Sud> Sudy { get; set; }
    public DbSet<Navrhovatel> Navrhovatelia { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Konanie>().Property(k => k.Id).ValueGeneratedNever();
        mb.Entity<Konanie>().HasOne(k => k.Sud).WithMany().HasForeignKey(k => k.SudId);
        mb.Entity<Konanie>().HasOne(k => k.Spravca).WithMany().HasForeignKey(k => k.SpravcaId);
        mb.Entity<Konanie>().HasOne(k => k.Dlznik).WithMany().HasForeignKey(k => k.DlznikId);

        mb.Entity<Sud>().Property(s => s.Id).ValueGeneratedNever();

        mb.Entity<Osoba>().HasIndex(o => o.Ico).HasDatabaseName("IX_Osoba_Ico");

        mb.Entity<Spravca>().HasIndex(s => s.Znacka).IsUnique().HasDatabaseName("IX_Spravca_Znacka");

        mb.Entity<Navrhovatel>().HasKey(n => new { n.KonanieId, n.OsobaId });
        mb.Entity<Navrhovatel>().HasOne(n => n.Konanie).WithMany(k => k.Navrhovatelia).HasForeignKey(n => n.KonanieId);
        mb.Entity<Navrhovatel>().HasOne(n => n.Osoba).WithMany().HasForeignKey(n => n.OsobaId);
    }
}
