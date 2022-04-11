using CoreModule.Web.Swagger;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables();
});

builder.Services.AddOcelot()
    .AddConsul();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Public Api Gateway" });

    // The XML comment files are copied using a post-build event (see project settings / Build Events).
    options.IncludeXmlDocumentationFromDirectory(AppDomain.CurrentDomain.BaseDirectory);

    // Optional but useful: this includes the summaries of the command and query types in the operations.
    options.IncludeMessageSummariesFromXmlDocs(AppDomain.CurrentDomain.BaseDirectory);
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Public Api Gateway v1"));
}

await app.UseOcelot();

app.Run();
