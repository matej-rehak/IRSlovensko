using IRSlovensko.Data;
using IRSlovensko.Services;
using Microsoft.EntityFrameworkCore;
using ServiceReference1;

namespace IRSlovensko;

class Program
{
    static async Task Main(string[] args)
    {

        var options = new DbContextOptionsBuilder<IRDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=IRSlovensko;Trusted_Connection=True;")
            .Options;

        await using var db = new IRDbContext(options);

        var konanieImporter = new KonanieImporter(db);
        await konanieImporter.ImportPoslednych30RokovAsync();

        var oznamImporter = new OznamImporter(db);
        await oznamImporter.ImportPoslednych30RokovAsync();
    }
}
