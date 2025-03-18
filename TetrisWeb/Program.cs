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
using System.Diagnostics.Metrics;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.Http;
using Prometheus;
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
    : "https://tetrisweb.azurewebsites.net";

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
          .AddAspNetCoreInstrumentation()
          .AddConsoleExporter())
      .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddMeter("http_requests_total", "player_score", "active_players", "api_response_time")
        .AddPrometheusExporter());


var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Service Name: {ServiceName} - Logging Initialized", serviceName);

var meter = new Meter(serviceName);

var requestCounter = meter.CreateCounter<long>("http_requests_total", "requests", "The total number of HTTP requests made.");
var requestCounter1 = meter.CreateObservableCounter<long>("http_requests_total", () => 0, "The total number of HTTP requests made.");
var playerScoreGauge = meter.CreateGauge<long>("player_score", "The score of the player");
var activePlayersCounter = meter.CreateUpDownCounter<int>("active_players");
var apiResponseTimeHistogram = meter.CreateHistogram<double>("api_response_time", "The response time of the API in milliseconds");

Task.Run(async () =>
{
    while (true)
    {
        var activePlayers = new Random().Next(0, 100);
        var responseTime = new Random().NextDouble() * 500;
        var playerScore = new Random().Next(0, 1000);

        requestCounter.Add(1);

        logger.LogInformation("Metric: http_requests_total, Value: {Value}", 1);
        logger.LogInformation("Metric: active_players, Value: {Value}", activePlayers);
        logger.LogInformation("Metric: api_response_time, Value: {Value}", responseTime);
        logger.LogInformation("Metric: player_score, Value: {Value}", playerScore);

        playerScoreGauge.Record(playerScore);
        activePlayersCounter.Add(activePlayers);
        apiResponseTimeHistogram.Record(responseTime);

        await Task.Delay(5000); 
    }
}); 
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapMetrics();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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
app.MapControllers();


app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();



app.Run("http://0.0.0.0:5000");

public partial class Program { }