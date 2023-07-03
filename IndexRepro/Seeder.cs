using IndexRepro.Domain;
using Marten;
using Marten.Schema;
using Marten.Schema.Identity;

namespace IndexRepro;

public class Seeder : IInitialData
{
    private static Genre Action = new Genre(Guid.Parse("2219b6f7-7883-4629-95d5-1a8a6c74b244"), "Action");
    private static Genre Adventure = new Genre(Guid.Parse("642a3e95-5875-498e-8ca0-93639ddfebcd"), "Adventure");
    private static Genre Indie = new Genre(Guid.Parse("331c15b4-b7bd-44d6-a804-b6879f99a65f"), "Indie");
    private static Genre RPG = new Genre(Guid.Parse("9d8ef25a-de9a-41e5-b72b-13f24b735883"), "RPG");
    private static Genre Simulation = new Genre(Guid.Parse("6337c55e-bb77-48e7-845b-5f42e009a410"), "Simulation");
    private static Genre Strategy = new Genre(Guid.Parse("a84bac0e-eb1f-4a06-883d-92d4539ffb0e"), "Strategy");
    private static Genre Casual = new Genre(Guid.Parse("fb592389-5b16-461c-a30d-94d9ed6493c9"), "Casual");
    private static Genre Sports = new Genre(Guid.Parse("ec7bf811-e219-4b11-92d3-b1dad5871cd3"), "Sports");
    private static Genre Free = new Genre(Guid.Parse("88291d85-3ad9-4952-9ff7-9a89c945fa6a"), "Free");
    private static Genre Racing = new Genre(Guid.Parse("61d3504e-ac0b-4330-9c52-44beed79ff76"), "Racing");
    private static Genre Education = new Genre(Guid.Parse("75e517b2-f803-44ea-81ac-559da44b66c0"), "Education");
    

    private static readonly IReadOnlyList<Genre> _genres = new List<Genre>
    {
        Action,
        Adventure,
        Indie,
        RPG,
        Simulation,
        Strategy,
        Casual,
        Sports,
        Free,
        Racing,
        Education
    };

    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        var hasGenres = await store.QuerySession().Query<Genre>().AnyAsync(cancellation);
        if (hasGenres)
            return;
        
        // Create Genres
        await store.BulkInsertAsync(
            documents: _genres,
            mode: BulkInsertMode.InsertsOnly,
            batchSize: 10_000,
            cancellation: cancellation);
        
        // Create Games
        const int TOTAL_GAMES = 1_000_000;
        
        var random = new Random();
        var games = new List<Game>(TOTAL_GAMES);

        for (var i = 0; i < TOTAL_GAMES; i++)
        {
            var genreIndex1 = random.Next(_genres.Count);
            var genreIndex2 = random.Next(_genres.Count);
            var genreIndex3 = random.Next(_genres.Count);
            
            var game = new Game(
                Id: CombGuidIdGeneration.NewGuid(),
                Name: $"Game #{i}",
                GenreIds: new Guid[]
                {
                    _genres[genreIndex1].Id, 
                    _genres[genreIndex2].Id,
                    _genres[genreIndex3].Id
                });
            
            games.Add(game);
        }
        
        await store.BulkInsertAsync(
            documents: games,
            mode: BulkInsertMode.InsertsOnly,
            batchSize: 10_000,
            cancellation: cancellation);
    }
}
