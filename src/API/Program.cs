using API.Extensions;
using Infrastructure.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.IdentityInitialiseDatabaseAsync();

    //app.UseSwagger();
    //app.UseSwaggerUI(op =>
    //{
    //    op.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    //});

    app.UseSwaggerUi(options =>
    {
        options.Path = "/api";
        options.DocumentPath = "/api/specification.json";
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
//app.UseRouting();
app.UseAuthorization();

app.MapFallbackToFile("index.html");
app.UseExceptionHandler(options => { });

//need to check
app.Map("/", () => Results.Redirect("/api"));

app.MapEndpoints();

app.Run();

