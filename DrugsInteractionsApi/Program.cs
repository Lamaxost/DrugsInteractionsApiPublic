using DrugsInteractionApi.Services.EntityFramework.Entities.DrugsCom.NewBase;
using DrugsInteractionApi.Services.EntityFramework.Entities.DrugsCom.OldBase;
using DrugsInteractionApi.Services.EntityFramework.Entities.Ktomalek;
using DrugsInteractionApi.Services.EntityFramework.Entities.NotFound;
using DrugsInteractionApi.Services.EntityFramework.Repositories;
using DrugsInteractionApi.Services.Repositories;
using DrugsInteractionsApi.MiddleWares;
using DrugsInteractionsApi.Services.EntityFramework.Entities.CheckMed;
using DrugsInteractionsApi.Services.EntityFramework.Entities.ComboMed;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Serilog;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});
Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
builder.Host.UseSerilog();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddScoped<IDrugsRepository, DrugsRepository>();

builder.Services.AddDbContextFactory<DrugsComContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("drugscom"));
});
builder.Services.AddDbContextFactory<DrugsCom2Context>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("drugscom2"));
});
builder.Services.AddDbContextFactory<CombomedOldContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("combomed"));
});
builder.Services.AddDbContextFactory<CombomedNewContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("combomed2"));
});

builder.Services.AddDbContextFactory<CheckMedContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("checkmed"));
});
builder.Services.AddDbContextFactory<KtomalekContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ktomalek"));
});

builder.Services.AddDbContextFactory<NotFoundContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("notfound"));
});


builder.Services.AddControllers().AddNewtonsoftJson();
builder.Configuration.AddJsonFile("appsettings.json");

var app = builder.Build();

app.UseHsts();
app.UseStaticFiles(new StaticFileOptions() { ServeUnknownFileTypes = true});
app.UseCors(MyAllowSpecificOrigins);
app.UseMiddleware<SimpleAuthMiddleware>();
app.MapControllers();

app.Run();
