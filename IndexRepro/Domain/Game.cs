using Marten;
using Weasel.Postgresql.Tables;

namespace IndexRepro.Domain;

public record Game(
    Guid Id,
    string Name,
    Guid[] GenreIds
);

public static class GameConfig
{
    public static void AddGameModule(this StoreOptions options)
    {
        options.RegisterDocumentType<Game>();

        options.Schema
            .For<Game>()
            .Index(o => o.Id, cfg =>
            {
                cfg.IsUnique = true;
            });
        
        options.Schema
            .For<Game>()
            .Index(o => o.GenreIds, cfg =>
            {
                cfg.Method = IndexMethod.gin;
            });
    }
}
