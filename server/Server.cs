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

        builder.Services.AddRazorPages(); // TODO: 

        WebApplication app = builder.Build();


        app.MapGet("/{s}/{p}", handleDefault);

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


    private static IResult handleCharacter(string id, IServiceProvider sp)
    {
        int int_id = int.Parse(id);

        Character? character = _characterService.GetCharacterById(int_id);

        if (character == null)
        {
            // null character exception
            Results.BadRequest();
        }

        var filledTemplate = RazorTemplateRenderer.RenderRazorViewToString("CharacterProfile", character, sp);

        return Results.Content(filledTemplate.ToString(), "text/html");

    }



    public static class RazorTemplateRenderer
    {
        public static async Task<string> RenderRazorViewToString<TModel>(
            string viewName,
            TModel model,
            IServiceProvider serviceProvider,
            Dictionary<string, object> viewDataDictionary = null)
        {
            var httpContext = new DefaultHttpContext { RequestServices = serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            // Resolve required services
            var viewEngine = serviceProvider.GetRequiredService<IRazorViewEngine>();
            var tempDataProvider = serviceProvider.GetRequiredService<ITempDataProvider>();
            var viewData = new ViewDataDictionary<TModel>(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary())
            {
                Model = model
            };

            // Add additional view data if provided
            if (viewDataDictionary != null)
            {
                foreach (var item in viewDataDictionary)
                {
                    viewData[item.Key] = item.Value;
                }
            }

            // Find the view
            var viewResult = viewEngine.FindView(actionContext, viewName, false);

            if (!viewResult.Success)
            {
                // Try again with view name as path
                viewResult = viewEngine.GetView(null, viewName, false);

                if (!viewResult.Success)
                {
                    throw new FileNotFoundException($"Could not find view '{viewName}'");
                }
            }

            // Render the view
            await using var writer = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                new TempDataDictionary(httpContext, tempDataProvider),
                writer,
                new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);
            return writer.ToString();
        }
    }

}