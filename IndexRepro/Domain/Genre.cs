using Marten;

namespace IndexRepro.Domain;

public record Genre(
    Guid Id,
    string Name
);


public static class GenreConfig
{
    public static void AddGenreModule(this StoreOptions options)
    {
        options.RegisterDocumentType<Genre>();

        options.Schema
            .For<Genre>()
            .Index(o => o.Id, cfg =>
            {
                cfg.IsUnique = true;
            });
    }
}
