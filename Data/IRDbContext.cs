using IRSlovensko.Models;
using IRSlovensko.Models.Ciselniky;
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
    public DbSet<KonanieOsoba> KonaniaOsoby { get; set; }
    public DbSet<Spravca> Spravcovia { get; set; }
    public DbSet<Sud> Sudy { get; set; }
    public DbSet<VerejnyOznam> VerejneOznamy { get; set; }
    public DbSet<Statistika> Statistika { get; set; }

    public DbSet<CSIRKonaniaTyp> CSIRKonaniaTyp { get; set; }
    public DbSet<CSIRKonaniaTypSpravcu> CSIRKonaniaTypSpravcu { get; set; }
    public DbSet<CSIRKonaniaTypPrideleniaSpravcu> CSIRKonaniaTypPrideleniaSpravcu { get; set; }
    public DbSet<CSIRKonaniaStavKonania> CSIRKonaniaStavKonania { get; set; }
    public DbSet<CSIRKonaniaDovodUkonceniaProcesu> CSIRKonaniaDovodUkonceniaProcesu { get; set; }
    public DbSet<CSIRVerejneOznamyOznamTyp> CSIRVerejneOznamyOznamTyp { get; set; }
    public DbSet<CSIRVerejneOznamyKonanieTyp> CSIRVerejneOznamyKonanieTyp { get; set; }
    public DbSet<CSIRVerejneOznamyDruhPodania> CSIRVerejneOznamyDruhPodania { get; set; }
    public DbSet<CSIRKonaniaOsobyTyp> CSIRKonaniaOsobyTyp { get; set; }
    public DbSet<CSIRKonaniaOsobyRole> CSIRKonaniaOsobyRole { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Konanie>().Property(k => k.Id).ValueGeneratedNever();
        mb.Entity<Konanie>().HasOne(k => k.Sud).WithMany().HasForeignKey(k => k.SudId);
        mb.Entity<Konanie>().HasOne(k => k.Spravca).WithMany().HasForeignKey(k => k.ZnackaSpravcu);
        mb.Entity<Konanie>().HasOne(k => k.Typ).WithMany().HasForeignKey(k => k.TypId);
        mb.Entity<Konanie>().HasOne(k => k.StavKonania).WithMany().HasForeignKey(k => k.StavKonaniaId);
        mb.Entity<Konanie>().HasOne(k => k.DovodUkonceniaProcesu).WithMany().HasForeignKey(k => k.DovodUkonceniaProcesuId);
        mb.Entity<Konanie>().HasOne(k => k.TypSpravcu).WithMany().HasForeignKey(k => k.TypSpravcuId);
        mb.Entity<Konanie>().HasOne(k => k.TypPrideleniaSpravcu).WithMany().HasForeignKey(k => k.TypPrideleniaSpravcuId);

        mb.Entity<Sud>().Property(s => s.Id).ValueGeneratedNever();

        mb.Entity<Spravca>().HasKey(s => s.Znacka);
        mb.Entity<Spravca>().Property(s => s.Znacka).ValueGeneratedNever();

        mb.Entity<KonanieOsoba>().HasIndex(o => o.Ico).HasDatabaseName("IX_KonanieOsoba_Ico");
        mb.Entity<KonanieOsoba>().HasOne(o => o.Konanie).WithMany(k => k.KonaniaOsoby).HasForeignKey(o => o.IdKonania);
        mb.Entity<KonanieOsoba>().HasOne(o => o.Typ).WithMany().HasForeignKey(o => o.TypId);
        mb.Entity<KonanieOsoba>().HasOne(o => o.Role).WithMany().HasForeignKey(o => o.RoleId);

        mb.Entity<VerejnyOznam>().HasKey(o => o.OznamId);
        mb.Entity<VerejnyOznam>().Property(o => o.OznamId).ValueGeneratedNever();
        mb.Entity<VerejnyOznam>().HasOne(o => o.Konanie).WithMany()
            .HasForeignKey(o => o.KonanieId)
            .OnDelete(DeleteBehavior.Restrict);
        mb.Entity<VerejnyOznam>().HasIndex(o => o.KonanieId).HasDatabaseName("IX_VerejnyOznam_KonanieId");
        mb.Entity<VerejnyOznam>().HasOne(o => o.OznamTyp).WithMany().HasForeignKey(o => o.OznamTypId);
        mb.Entity<VerejnyOznam>().HasOne(o => o.KonanieTyp).WithMany().HasForeignKey(o => o.KonanieTypId);
        mb.Entity<VerejnyOznam>().HasOne(o => o.DruhPodania).WithMany().HasForeignKey(o => o.DruhPodaniaId);

        mb.Entity<Statistika>().HasKey(s => s.DatumStahovania);
        mb.Entity<Statistika>().Property(s => s.DatumStahovania).ValueGeneratedNever();

        // Číselník CSIRKonaniaTyp
        mb.Entity<CSIRKonaniaTyp>().HasData(
            new CSIRKonaniaTyp { Id = 1, Nazov = "Likvidácia" },
            new CSIRKonaniaTyp { Id = 2, Nazov = "Iné" },
            new CSIRKonaniaTyp { Id = 3, Nazov = "Reštrukturalizácia" },
            new CSIRKonaniaTyp { Id = 4, Nazov = "Oddĺženie - splátkový kalendár" },
            new CSIRKonaniaTyp { Id = 5, Nazov = "Oddĺženie - konkurz" },
            new CSIRKonaniaTyp { Id = 6, Nazov = "Malý konkurz" },
            new CSIRKonaniaTyp { Id = 7, Nazov = "Konkurz" },
            new CSIRKonaniaTyp { Id = 8, Nazov = "Verejná preventívna reštrukturalizácia" }
        );

        // Číselník CSIRKonaniaTypSpravcu
        mb.Entity<CSIRKonaniaTypSpravcu>().HasData(
            new CSIRKonaniaTypSpravcu { Id = 1, Nazov = "Riadny" },
            new CSIRKonaniaTypSpravcu { Id = 2, Nazov = "Dozorný" },
            new CSIRKonaniaTypSpravcu { Id = 3, Nazov = "Predbežný" }
        );

        // Číselník CSIRKonaniaTypPrideleniaSpravcu
        mb.Entity<CSIRKonaniaTypPrideleniaSpravcu>().HasData(
            new CSIRKonaniaTypPrideleniaSpravcu { Id = 1, Nazov = "Vymenovaný" },
            new CSIRKonaniaTypPrideleniaSpravcu { Id = 2, Nazov = "Generovaný" }
        );

        // Číselník CSIRKonaniaStavKonania
        mb.Entity<CSIRKonaniaStavKonania>().HasData(
            new CSIRKonaniaStavKonania { Id = 1, Nazov = "Prebiehajúce konanie" },
            new CSIRKonaniaStavKonania { Id = 2, Nazov = "Prerušené konanie" },
            new CSIRKonaniaStavKonania { Id = 3, Nazov = "Skončený proces" },
            new CSIRKonaniaStavKonania { Id = 4, Nazov = "Začaté konanie" },
            new CSIRKonaniaStavKonania { Id = 5, Nazov = "Začatý proces konania" },
            new CSIRKonaniaStavKonania { Id = 6, Nazov = "Zastavené konanie" }
        );

        // Číselník CSIRKonaniaDovodUkonceniaProcesu
        mb.Entity<CSIRKonaniaDovodUkonceniaProcesu>().HasData(
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 1, Nazov = "Inak" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 2, Nazov = "Iný dôvod" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 3, Nazov = "Nedá sa zostaviť splátkový kalendár správcom" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 4, Nazov = "Nedostatok majetku" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 5, Nazov = "Nesplnenie podmienok" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 6, Nazov = "Odmietnutie" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 7, Nazov = "Odsúhlasený plán" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 8, Nazov = "Osvedčená platobná schopnosť" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 9, Nazov = "Potvrdenie plánu súdom" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 10, Nazov = "Povolenie reštrukturalizácie" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 11, Nazov = "Skončené oddĺženie" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 12, Nazov = "Späťvzatie" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 13, Nazov = "Splnenie konečného rozvrhu" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 14, Nazov = "Splnenie rozvrhu výťažku" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 15, Nazov = "Určený splátkový kalendár" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 16, Nazov = "Vyhlásenie konkurzu" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 17, Nazov = "Zaplatenie splatných pohľadávok" },
            new CSIRKonaniaDovodUkonceniaProcesu { Id = 18, Nazov = "Žiadny veriteľ" }
        );

        // Číselník CSIRVerejneOznamyOznamTyp
        mb.Entity<CSIRVerejneOznamyOznamTyp>().HasData(
            new CSIRVerejneOznamyOznamTyp { Id = 1, Nazov = "Oznám súdu" },
            new CSIRVerejneOznamyOznamTyp { Id = 2, Nazov = "Oznám správcu" }
        );

        // Číselník CSIRVerejneOznamyKonanieTyp
        mb.Entity<CSIRVerejneOznamyKonanieTyp>().HasData(
            new CSIRVerejneOznamyKonanieTyp { Id = 1, Nazov = "Konkurz" },
            new CSIRVerejneOznamyKonanieTyp { Id = 2, Nazov = "Malý konkurz" },
            new CSIRVerejneOznamyKonanieTyp { Id = 3, Nazov = "Reštrukturalizácia" },
            new CSIRVerejneOznamyKonanieTyp { Id = 4, Nazov = "Iné konania" },
            new CSIRVerejneOznamyKonanieTyp { Id = 5, Nazov = "Oddĺženie - konkurz" },
            new CSIRVerejneOznamyKonanieTyp { Id = 6, Nazov = "Oddĺženie - splátkový kalendár" },
            new CSIRVerejneOznamyKonanieTyp { Id = 7, Nazov = "Likvidácia" },
            new CSIRVerejneOznamyKonanieTyp { Id = 8, Nazov = "Verejná preventívna reštrukturalizácia" }
        );

        // Číselník CSIRVerejneOznamyDruhPodania
        mb.Entity<CSIRVerejneOznamyDruhPodania>().HasData(
            new CSIRVerejneOznamyDruhPodania { Id = 1, Nazov = "Oznám o mieste a čase nahliadnutia do spisu" },
            new CSIRVerejneOznamyDruhPodania { Id = 2, Nazov = "Oznám o zvolaní schôdze veriteľov" },
            new CSIRVerejneOznamyDruhPodania { Id = 3, Nazov = "Zápisnica zo zasadnutia veriteľského výboru" },
            new CSIRVerejneOznamyDruhPodania { Id = 4, Nazov = "Oznámenia súvisiace s dražbou majetku" },
            new CSIRVerejneOznamyDruhPodania { Id = 5, Nazov = "Súpis všeobecnej podstaty" },
            new CSIRVerejneOznamyDruhPodania { Id = 6, Nazov = "Súpis oddelenej podstaty" },
            new CSIRVerejneOznamyDruhPodania { Id = 7, Nazov = "Zverejnenie návrhu čiastkového rozvrhu (všeobecná podstata)" },
            new CSIRVerejneOznamyDruhPodania { Id = 8, Nazov = "Zverejnenie návrhu konečného výťažku (všeobecná podstata)" },
            new CSIRVerejneOznamyDruhPodania { Id = 9, Nazov = "Doplnenie súpisu všeobecnej podstaty" },
            new CSIRVerejneOznamyDruhPodania { Id = 10, Nazov = "Doplnenie súpisu oddelenej podstaty" },
            new CSIRVerejneOznamyDruhPodania { Id = 11, Nazov = "Preradenie súpisovej zložky" },
            new CSIRVerejneOznamyDruhPodania { Id = 12, Nazov = "Pripísanie poznámky o spornom zápise" },
            new CSIRVerejneOznamyDruhPodania { Id = 13, Nazov = "Doplnenie alebo zmena poznámky o spornom zápise" },
            new CSIRVerejneOznamyDruhPodania { Id = 14, Nazov = "Výmaz poznámky o spornom zápise" },
            new CSIRVerejneOznamyDruhPodania { Id = 15, Nazov = "Vylúčenie súpisovej zložky majetku zo súpisu" },
            new CSIRVerejneOznamyDruhPodania { Id = 16, Nazov = "Iné zverejnenie" },
            new CSIRVerejneOznamyDruhPodania { Id = 17, Nazov = "Výzva zahraničným veriteľom na prihlásenie pohľadávok" },
            new CSIRVerejneOznamyDruhPodania { Id = 18, Nazov = "Oznámenie čísla bankového účtu pre potreby popierania pohľadávok" },
            new CSIRVerejneOznamyDruhPodania { Id = 19, Nazov = "Oznámenie o doručení prihlášky po uplynutí základnej prihlasovacej lehoty" },
            new CSIRVerejneOznamyDruhPodania { Id = 20, Nazov = "Oznámenia súvisiace so speňažovaním majetku" },
            new CSIRVerejneOznamyDruhPodania { Id = 21, Nazov = "Oznámenie o zostavení zoznamu pohľadávok (všeobecná podstata)" },
            new CSIRVerejneOznamyDruhPodania { Id = 22, Nazov = "Oznámenie o zostavení zoznamu pohľadávok (oddelená podstata)" },
            new CSIRVerejneOznamyDruhPodania { Id = 23, Nazov = "Zverejnenie návrhu čiastkového rozvrhu (oddelená podstata)" },
            new CSIRVerejneOznamyDruhPodania { Id = 24, Nazov = "Zverejnenie návrhu konečného rozvrhu (oddelená podstata)" },
            new CSIRVerejneOznamyDruhPodania { Id = 25, Nazov = "Zápisnica zo zasadnutia so zástupcom veriteľov" },
            new CSIRVerejneOznamyDruhPodania { Id = 26, Nazov = "Oznám o zostavení návrhu splátkového kalendára" },
            new CSIRVerejneOznamyDruhPodania { Id = 27, Nazov = "Oznám o nemožnosti zostaviť návrh splátkového kalendára" },
            new CSIRVerejneOznamyDruhPodania { Id = 28, Nazov = "Oznám o zrušení konkurzu - splnenie rozvrhu výťažku" },
            new CSIRVerejneOznamyDruhPodania { Id = 29, Nazov = "Oznám o zrušení konkurzu - podstata nepokryje náklady" },
            new CSIRVerejneOznamyDruhPodania { Id = 30, Nazov = "Oznám o zrušení konkurzu - žiadny veriteľ do 90 dní" },
            new CSIRVerejneOznamyDruhPodania { Id = 31, Nazov = "Oznám o zrušení konkurzu - zánik postavenia veriteľov" },
            new CSIRVerejneOznamyDruhPodania { Id = 32, Nazov = "Oznám o skončení konania o návrhu na určenie SK" },
            new CSIRVerejneOznamyDruhPodania { Id = 33, Nazov = "Oznám o uplatnení námietky voči zápisu majetku do súpisu" },
            new CSIRVerejneOznamyDruhPodania { Id = 34, Nazov = "Prebiehajúca likvidácia" },
            new CSIRVerejneOznamyDruhPodania { Id = 35, Nazov = "Likvidácia - prihlasovanie pohľadávok" },
            new CSIRVerejneOznamyDruhPodania { Id = 36, Nazov = "Likvidácia - vyhotovenie zoznamu pohľadávok" },
            new CSIRVerejneOznamyDruhPodania { Id = 37, Nazov = "Likvidácia - vyhotovenie zoznamu majetku" },
            new CSIRVerejneOznamyDruhPodania { Id = 38, Nazov = "Podanie návrhu na vyhlásenie konkurzu (likvidátor)" },
            new CSIRVerejneOznamyDruhPodania { Id = 39, Nazov = "Prerušená likvidácia - vyhlásený konkurz" },
            new CSIRVerejneOznamyDruhPodania { Id = 40, Nazov = "Prerušená likvidácia" },
            new CSIRVerejneOznamyDruhPodania { Id = 41, Nazov = "Skončené oznámením likvidátora" },
            new CSIRVerejneOznamyDruhPodania { Id = 42, Nazov = "Schválenie účtovnej závierky a konečnej správy" },
            new CSIRVerejneOznamyDruhPodania { Id = 43, Nazov = "Prebiehajúca dodatočná likvidácia" },
            new CSIRVerejneOznamyDruhPodania { Id = 44, Nazov = "Dodatočná likvidácia - prihlasovanie pohľadávok" },
            new CSIRVerejneOznamyDruhPodania { Id = 45, Nazov = "Dodatočná likvidácia - vyhotovenie zoznamu pohľadávok" },
            new CSIRVerejneOznamyDruhPodania { Id = 46, Nazov = "Dodatočná likvidácia - vyhotovenie zoznamu majetku" },
            new CSIRVerejneOznamyDruhPodania { Id = 47, Nazov = "Dodatočná likvidácia - podanie návrhu na vyhlásenie konkurzu" },
            new CSIRVerejneOznamyDruhPodania { Id = 48, Nazov = "Dodatočná likvidácia - prerušené, vyhlásený konkurz" },
            new CSIRVerejneOznamyDruhPodania { Id = 49, Nazov = "Dodatočná likvidácia - prerušené" },
            new CSIRVerejneOznamyDruhPodania { Id = 50, Nazov = "Dodatočná likvidácia - skončené oznámením likvidátora" },
            new CSIRVerejneOznamyDruhPodania { Id = 51, Nazov = "Dodatočná likvidácia - schválenie účtovnej závierky" },
            new CSIRVerejneOznamyDruhPodania { Id = 52, Nazov = "Zvolanie schvaľovacej schôdze veriteľov po schválení plánu" },
            new CSIRVerejneOznamyDruhPodania { Id = 53, Nazov = "Správa o postupe speňažovania" },
            new CSIRVerejneOznamyDruhPodania { Id = 54, Nazov = "Zápisnica zo schôdze veriteľov" },
            new CSIRVerejneOznamyDruhPodania { Id = 55, Nazov = "Zápisnica zo schvaľovacej schôdze veriteľov" },
            new CSIRVerejneOznamyDruhPodania { Id = 56, Nazov = "Zvolanie schôdze veriteľského výboru na schválenie plánu" },
            new CSIRVerejneOznamyDruhPodania { Id = 57, Nazov = "Záznam z online schôdze veriteľov" },
            new CSIRVerejneOznamyDruhPodania { Id = 58, Nazov = "Oznámenie o zverejnení súpisu majetku" },
            new CSIRVerejneOznamyDruhPodania { Id = 59, Nazov = "Ukončenie prihlasovania pohľadávok" },
            new CSIRVerejneOznamyDruhPodania { Id = 60, Nazov = "Ukončenie prieskumu pohľadávok" },
            new CSIRVerejneOznamyDruhPodania { Id = 61, Nazov = "Nedefinované" }
        );

        // Číselník CSIRKonaniaOsobyTyp
        mb.Entity<CSIRKonaniaOsobyTyp>().HasData(
            new CSIRKonaniaOsobyTyp { Id = 1, Nazov = "Fyzická osoba" },
            new CSIRKonaniaOsobyTyp { Id = 2, Nazov = "Fyzická osoba - podnikateľ" },
            new CSIRKonaniaOsobyTyp { Id = 3, Nazov = "Právnická osoba" }
        );

        // Číselník CSIRKonaniaOsobyRole
        mb.Entity<CSIRKonaniaOsobyRole>().HasData(
            new CSIRKonaniaOsobyRole { Id = 1, Nazov = "Dlžník" },
            new CSIRKonaniaOsobyRole { Id = 2, Nazov = "Navrhovateľ" }
        );
    }
}
