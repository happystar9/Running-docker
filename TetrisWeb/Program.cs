using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using TetrisWeb.Components;
using TetrisWeb.Components.Account;
using TetrisWeb.GameData;
using TetrisWeb.ApiServices;
using TetrisWeb.AuthData;
using Microsoft.AspNetCore.Builder;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.DTOs;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
        policy.WithOrigins("https://tetrisweb.azurewebsites.net")
              .AllowAnyMethod()
              .AllowAnyHeader());
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration["DB_CONN"] ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


string apiBaseUrl = builder.Environment.IsDevelopment()
    ? "http://localhost:5247"
    : builder.Configuration["ApiBaseUrl"];

builder.Services.AddHttpClient("TetrisApi", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});


//builder.Services.AddHttpClient("TetrisApi", client =>
//{
//    client.BaseAddress = new Uri("http://localhost:5247");
//});



builder.Services.AddDbContext<Dbf25TeamArzContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IApiKeyManagementService, ApiKeyManagementService>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddTransient<IGameService, GameService>();
builder.Services.AddSingleton<GameSessionService>();
builder.Services.AddTransient<GameSessionDto>();
builder.Services.AddScoped<IScoreService, ScoreService>();
//builder.Services.AddTransient<GameManager>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();


var connectionString2 = builder.Configuration["DB_CONN"] ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString2, o => o.MigrationsHistoryTable("__EFMigrationsHistory", "authentication")));

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
});


builder.Services.AddTransient<IEmailSender<ApplicationUser>, EmailSender>();

var serviceName = "TetrisWebService";

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
        .AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Endpoint = new Uri("http://otel-collector:4317");  
            otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc; 
        })
        .AddConsoleExporter();
});
builder.Services.AddOpenTelemetry()
      .ConfigureResource(resource => resource.AddService(serviceName))
      .WithTracing(tracing => tracing
          .AddConsoleExporter())
      .WithMetrics(metrics => metrics
          .AddConsoleExporter());

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Service Name: {ServiceName} - Logging Initialized", serviceName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();



app.Run("http://0.0.0.0:5000");

public partial class Program { }
