using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;  // This was missing (for ViewContext)
using Microsoft.AspNetCore.Mvc.ViewEngines; // This was missing (for IViewEngine)
using Microsoft.EntityFrameworkCore;

namespace ProjectSolamnia.Server;

public class SolamniaServer
{

    private static ServiceProvider provider = null!;
    private static ProjectSolamniaDbContext _dbContext = null!;
    private static TraitService _traitService = null!;
    private static CharacterService _characterService = null!;
    private static HoldingService _holdingService = null!;



    public static void Run()
    {

        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); // TODO: 
        builder.Services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

        WebApplication app = builder.Build();

        InitializeServices();

        app.MapGet("/c/{id}", handleCharacter);
        app.MapGet("/t/{id}", handleTrait);

        app.Run();

    }

    private static void InitializeServices()
    {
        var services = new ServiceCollection();

        services.AddDbContext<ProjectSolamniaDbContext>(options =>
            options.UseSqlite("Data Source=solamnia.db"));
        services.AddScoped<TraitService>();
        services.AddScoped<CharacterService>();
        services.AddScoped<HoldingService>();

        provider = services.BuildServiceProvider();

        var scope = provider.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<ProjectSolamniaDbContext>();
        _dbContext.Database.Migrate();

        _traitService = scope.ServiceProvider.GetRequiredService<TraitService>();
        _characterService = scope.ServiceProvider.GetRequiredService<CharacterService>();
        _holdingService = scope.ServiceProvider.GetRequiredService<HoldingService>();
    }

    private static void handleDefault(string p, string s)
    {
        if (p != null && s != null)
        {
            Console.WriteLine(s + " " + p);
        }
    }


    private static async Task<IResult> handleCharacter(string id, IRazorViewToStringRenderer renderer)
    {
        if (!int.TryParse(id, out var intId))
        {
            return Results.BadRequest("Invalid ID format");
        }

        Character? character = _characterService.GetCharacterById(intId);
        if (character == null)
        {
            return Results.NotFound();
        }

        try
        {
            // Try different view paths if needed
            var viewPaths = new[]
            {
                "/Views/CharacterProfile.cshtml",
                "Views/CharacterProfile.cshtml",
                "CharacterProfile.cshtml"
            };

            foreach (var viewPath in viewPaths)
            {
                try
                {
                    var html = await renderer.RenderViewToStringAsync(viewPath, character);
                    return Results.Content(html, "text/html");
                }
                catch { /* Try next path */ }
            }

            return Results.Problem("Could not find the view file");
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error rendering view: {ex.Message}");
        }
    }


    private static async Task<IResult> handleTrait(string id, IRazorViewToStringRenderer renderer)
    {
        if (!int.TryParse(id, out var intId))
        {
            return Results.BadRequest("Invalid ID format");
        }

        Trait? trait = _traitService.GetTraitById(intId);
        if ( trait == null)
        {
            return Results.NotFound();
        }

        try
        {
            // Try different view paths if needed
            var views = new[]
            {
                "/Views/TraitPage.cshtml",
                "Views/TraitPage.cshtml",
                "TraitPage.cshtml",
            };

            foreach (var viewPath in views)
            {
                try
                {
                    var html = await renderer.RenderViewToStringAsync(viewPath, trait);
                    return Results.Content(html, "text/html");
                }
                catch (Exception e)
                { /* Try next path */
                    Console.WriteLine(viewPath + ": " + e.Message);
                }
            }

            return Results.Problem("Could not find the view file");
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error rendering view: {ex.Message}");
        }
    }
    

}