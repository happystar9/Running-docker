using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.GameData;
using TetrisWeb.DTOs;

namespace TetrisWeb.ApiServices;

public class ApiKeyValidationService : IApiKeyValidationService
{
    private readonly Dbf25TeamArzContext _dbContext;

    public ApiKeyValidationService(Dbf25TeamArzContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool IsValidApiKey(string key)
    {
        var apiKey = _dbContext.ApiKeys.FirstOrDefault(k => k.Key == key && (k.ExpiredOn == null || k.ExpiredOn > DateTime.Now));
        return apiKey != null;
    }

    public Task AssignKeyAsync(PlayerDto player, string key)
    {
        var newApiKey = new ApiKey
        {
            Key = key,
            PlayerId = player.Id,
            CreatedOn = DateTime.Now,
            ExpiredOn = DateTime.Now.AddDays(1)
        };

        _dbContext.ApiKeys.Add(newApiKey);
        return _dbContext.SaveChangesAsync();
    }

    public Task InvalidateApiKeyAsync(string key)
    {
        var apiKey = _dbContext.ApiKeys.FirstOrDefault(k => k.Key == key);
        if (apiKey != null)
        {
            apiKey.ExpiredOn = DateTime.Now;
        }

        return _dbContext.SaveChangesAsync();
    }

}
