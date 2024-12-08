using TetrisWeb.GameData;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace TetrisTest.IntegrationTests;

public abstract class PostgresTestBase : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    protected WebApplicationFactory<Program> CustomWebAppFactory { get; }
    protected static PostgreSqlContainer DbContainer { get; private set; }
    protected ITestOutputHelper OutputHelper { get; }
    protected IServiceScope? Scope { get; private set; }

    public PostgresTestBase(WebApplicationFactory<Program> webAppFactory, ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;

        DbContainer ??= new PostgreSqlBuilder()
    .WithImage("postgres:13")
    .WithPortBinding(1646, 5432)
    .WithPassword("Strong_password_123!")
    .Build();


        CustomWebAppFactory = webAppFactory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("DB_CONN", DbContainer.GetConnectionString());
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<Dbf25TeamArzContext>();
                services.RemoveAll<DbContextOptions>();
                services.RemoveAll(typeof(DbContextOptions<Dbf25TeamArzContext>));
                services.AddDbContext<Dbf25TeamArzContext>(options => options.UseNpgsql(DbContainer.GetConnectionString()));
            });
        });
    }

    public async Task InitializeAsync()
    {
        await DbContainer.StartAsync();

        var projectRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName).FullName;
        var schemaFilePath = Path.Combine(projectRoot, "schema.sql");
        OutputHelper.WriteLine($"Schema file path: {schemaFilePath}");
        var schemaScript = await File.ReadAllTextAsync(schemaFilePath);
        await DbContainer.ExecScriptAsync(schemaScript);
    }

    public async Task DisposeAsync()
    {
        Scope?.Dispose();
        await DbContainer.StopAsync();
    }

    protected T GetService<T>() where T : notnull
    {
        Scope = CustomWebAppFactory.Services.CreateScope();
        return Scope.ServiceProvider.GetRequiredService<T>();
    }
}


[CollectionDefinition("SequentialTestExecution", DisableParallelization = true)]
public class SequentialTestExecutionDefinition { }