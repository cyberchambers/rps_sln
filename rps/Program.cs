using Microsoft.EntityFrameworkCore;
using rps;
using System.Data;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.OpenApi.Any;

var rps_builder = WebApplication.CreateBuilder(args);
rps_builder.Services.AddDbContext<RpsDb>(opt => opt.UseInMemoryDatabase("RpsMatch"));
rps_builder.Services.AddDatabaseDeveloperPageExceptionFilter();
rps_builder.Services.AddMvc();
rps_builder.Services.AddEndpointsApiExplorer();
rps_builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Roshambo API",
        Description = "An ASP.NET Core Web API for a game of Roshambo (rock/paper/scissors)",
        Contact = new OpenApiContact
        {
            Name = "Mike Chambers",
            Email = "mike@cyberchambers.org",
            Url = new Uri("https://www.linkedin.com/in/cyberchambers/")
        },

    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
});
rps_builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.IncludeFields= true;
});
Random rand = new Random();
var rps_app = rps_builder.Build();

rps_app.MapPost("/throw/{rules}&{playername}&{playerthrow}", async (string rules, string playername, string playerthrow, RpsMatch rpsmatch, RpsDb db) =>
{
    // randomly pick from the rules dictionary
    // assign this value to the AppThrow
    rpsmatch.Rules = rules;
    var rules_dict = JsonSerializer.Deserialize<IDictionary<string, string>>(rules);
    int index = rand.Next(rules_dict.Count);
    var rule = rules_dict.ElementAt(index);
    rpsmatch.AppThrow = rule.Key;
    string losesto = rule.Value;
    string appthrow = rpsmatch.AppThrow;
    rpsmatch.PlayerName = playername;
    rpsmatch.PlayerThrow = playerthrow;

    //evaluate the contest
    if (losesto.Contains(playerthrow))
    {
        rpsmatch.Winner = rpsmatch.PlayerName;
    }
    else if (playerthrow == appthrow)
    {
        rpsmatch.Winner = "Draw";
    }
    else
    {
        rpsmatch.Winner = "Computer";
    }

    db.RpsMatches.Add(rpsmatch);

    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{rpsmatch.Id}", rpsmatch);
})
    .WithName("throwdown")
    .WithOpenApi(generatedOperation =>
    {
        var param0 = generatedOperation.Parameters[0];
        param0.Description = "A JSON dictionary representing possible choices and what defeats each choice.";
        param0.Example = new OpenApiString("{\"rock\":\"paper\",\"paper\":\"scissors\",\"scissors\":\"rock\"}");

        var param1 = generatedOperation.Parameters[1];
        param1.Description = "A name you would like to use for this game.";
        param1.Example = new OpenApiString("Cantankerous Frank");

        var param2 = generatedOperation.Parameters[2];
        param2.Description = "Your \"throw\". i.e. rock, paper, or scissors.";
        
        return generatedOperation;
    });

rps_app.MapGet("/", async (RpsDb db) =>
    await db.RpsMatches.ToListAsync())
    .WithName("RpsUrlRoot");

rps_app.MapGet("/games", async (RpsDb db) =>
    await db.RpsMatches.ToListAsync());

rps_app.MapGet("/wins/{playername}", async (string playername, RpsDb db) =>
    await db.RpsMatches.Where(p => p.Winner == playername).ToListAsync());

rps_app.MapGet("/draws", async (RpsDb db) =>
    await db.RpsMatches.Where(t => t.Winner.StartsWith("Draw")).ToListAsync());

// Put a lil swag on it...
rps_app.UseSwagger();
rps_app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
rps_app.Run();