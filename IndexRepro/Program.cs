using IndexRepro;
using IndexRepro.Domain;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Oakton;
using Wolverine;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ApplyOaktonExtensions();
builder.Host.UseWolverine(opts =>
{
    opts.OptimizeArtifactWorkflow();
    opts.Policies.AutoApplyTransactions();

    opts.Services.AddMarten(marten =>
    {
        var connectionString = builder
            .Configuration
            .GetConnectionString("postgres")!;

        marten.Connection(connectionString);
        
        // Configure Documents
        marten.AddGameModule();
        marten.AddGenreModule();
    })
    .InitializeWith<Seeder>()
    .OptimizeArtifactWorkflow()
    .UseLightweightSessions()
    .AddAsyncDaemon(DaemonMode.HotCold)
    .IntegrateWithWolverine();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*
 * Attempts to query games where the given 'genreId' is in the 'GenreIds' array using '.Contains()'
 */
app.MapGet("/games/contains-query", (Guid? genreId, int? skip, int? take, IQuerySession session) =>
{
    var genreIdValue = genreId ?? Guid.Parse("61d3504e-ac0b-4330-9c52-44beed79ff76"); // Racing Genre
    var dbQuery = session
        .Query<Game>()
        .Where(o => o.GenreIds.Contains(genreIdValue))
        .Skip(skip ?? 0)
        .Take(take ?? 100);

    // var sql = dbQuery.ToCommand();
    // var queryPlan = dbQuery.Explain();

    var games = dbQuery.ToList();
    return Results.Ok(games);
});

/*
 * Attempts to query games where the given 'genreId' is in the 'GenreIds' array using '.Any()'
 */
app.MapGet("/games/any-query", (Guid? genreId, int? skip, int? take, IQuerySession session) =>
{
    var genreIdValue = genreId ?? Guid.Parse("61d3504e-ac0b-4330-9c52-44beed79ff76"); // Racing Genre
    var dbQuery = session
        .Query<Game>()
        .Where(o => o.GenreIds.Any(id => id == genreIdValue))
        .Skip(skip ?? 0)
        .Take(take ?? 100);

    // var sql = dbQuery.ToCommand();
    // var queryPlan = dbQuery.Explain();

    var games = dbQuery.ToList();
    return Results.Ok(games);
});

/*
 * Attempts to query games where the given 'genreId' is in the 'GenreIds' array using '.IsOneOf()'
 */
app.MapGet("/games/is-one-of-query", (Guid? genreId, int? skip, int? take, IQuerySession session) =>
{
    var genreIdValue = genreId ?? Guid.Parse("61d3504e-ac0b-4330-9c52-44beed79ff76"); // Racing Genre
    var genreIdArray = new Guid[] { genreIdValue };
    
    // NOTE: This usage actually throws a runtime exception so I was not
    // able to test if the index was used with this approach.
    var dbQuery = session
        .Query<Game>()
        .Where(o => o.GenreIds.IsOneOf(genreIdArray))
        .Skip(skip ?? 0)
        .Take(take ?? 100);

    // var sql = dbQuery.ToCommand();
    // var queryPlan = dbQuery.Explain();

    var games = dbQuery.ToList();
    return Results.Ok(games);
});

return await app.RunOaktonCommands(args);
