using IRSlovensko.Data;
using IRSlovensko.Services;
using Microsoft.EntityFrameworkCore;

namespace IRSlovensko;

class Program
{
    static async Task Main(string[] args)
    {
        var options = new DbContextOptionsBuilder<IRDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=IRSlovensko;Trusted_Connection=True;")
            .Options;

        await using var db = new IRDbContext(options);
        var importer = new KonanieImporter(db);
        await importer.ImportPoslednych30RokovAsync();
    }
}
