using Oakton;
using SteamDependencyRepro.Events;
using SteamDependencyRepro.Features.FetchPackage;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ApplyOaktonExtensions();
builder.Host.UseWolverine(opts =>
{
    opts.OptimizeArtifactWorkflow();
    opts.Policies.AutoApplyTransactions();

    opts.LocalQueueFor<FetchPackageCommand>();
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

app.MapPost("/packages/fetch", (IMessageBus bus) => bus.InvokeAsync<PackageFetched>(new FetchPackageCommand(12345)));

return await app.RunOaktonCommands(args);