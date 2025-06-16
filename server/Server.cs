using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectSolamnia.Server;

public class SolamniaServer
{
    public static void Run()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        builder.Services.AddRazorPages(); // TODO: 

        WebApplication app = builder.Build();


        app.MapGet("/{s}/{p}", handleDefault);

        app.Run();

    }

    private static void handleDefault(string p, string s)
    {
        if (p != null && s != null)
        {
            Console.WriteLine(s + " " + p);
        }
    }
}