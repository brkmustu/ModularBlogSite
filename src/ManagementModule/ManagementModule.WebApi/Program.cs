﻿using Microsoft.EntityFrameworkCore;
using ManagementModule;
using ManagementModule.System.SeedSampleData;
using CoreModule.Web.Swagger;
using Microsoft.OpenApi.Models;
using CoreModule.Web.Codes;
using SimpleInjector;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => { lc.ReadFrom.Configuration(ctx.Configuration); });

var services = builder.Services;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Open http://localhost:5132/swagger/ to browse the API.
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Management Module Services API" });

    // The XML comment files are copied using a post-build event (see project settings / Build Events).
    options.IncludeXmlDocumentationFromDirectory(AppDomain.CurrentDomain.BaseDirectory);

    // Optional but useful: this includes the summaries of the command and query types in the operations.
    options.IncludeMessageSummariesFromXmlDocs(AppDomain.CurrentDomain.BaseDirectory);
});

services.AddApplication(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddWebApi(builder.Configuration);

var container = new Container();

services.AddSimpleInjector(container, options =>
{
    options.AddAspNetCore();
});

Bootstrapper.Bootstrap(container, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var scopedServices = scope.ServiceProvider;

    try
    {
        var baseModuleContext = scopedServices.GetRequiredService<ManagementModuleDbContext>();
        baseModuleContext.Database.Migrate();

        var sampleDataSeeder = scopedServices.GetRequiredService<SampleDataSeeder>();
        await sampleDataSeeder.SeedAllAsync(CancellationToken.None);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or initializing the database.");
    }
}

app.MapCommands(
    pattern: MessageMapping.FlatApi(new Commands(container), "/api/management/commands/{0}"),
    commandTypes: Bootstrapper.GetKnownCommandTypes());
app.MapQueries(
    pattern: MessageMapping.FlatApi(new Queries(container), "/api/management/queries/{0}"),
    queryTypes: Bootstrapper.GetKnownQueryTypes());

app.Run();
