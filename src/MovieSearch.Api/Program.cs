using System.Reflection;
using Ben.Diagnostics;
using BuildingBlocks.Logging;
using BuildingBlocks.Resiliency.Configs;
using BuildingBlocks.Swagger;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieSearch.Application;
using MovieSearch.Core;
using MovieSearch.Infrastructure;
using Serilog;

// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis
// https://benfoster.io/blog/mvc-to-minimal-apis-aspnet-6/
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(
    (env, c) =>
    {
        // Handling Captive Dependency Problem
        // https://ankitvijay.net/2020/03/17/net-core-and-di-beware-of-captive-dependency/
        // https://levelup.gitconnected.com/top-misconceptions-about-dependency-injection-in-asp-net-core-c6a7afd14eb4
        // https://blog.ploeh.dk/2014/06/02/captive-dependency/
        if (
            env.HostingEnvironment.IsDevelopment()
            || env.HostingEnvironment.IsEnvironment("tests")
            || env.HostingEnvironment.IsStaging()
        )
            c.ValidateScopes = true;
    }
);

builder.AddCustomSerilog();

builder.Services.AddControllers(options =>
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()))
);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "api",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

builder.Services.AddApplication();
builder.AddInfrastructure();

var app = builder.Build();

app.UseInfrastructure(app.Environment);

app.UseCors("api");

//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-7.0#routing-basics
// app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", context => context.Response.WriteAsync("Movie Search Api!"));

if (app.Environment.IsDevelopment())
    app.UseCustomSwagger();

await app.RunAsync();

public partial class Program { }
