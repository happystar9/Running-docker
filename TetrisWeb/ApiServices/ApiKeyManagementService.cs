using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.GameData;
using TetrisWeb.DTOs;
using Microsoft.EntityFrameworkCore;

namespace TetrisWeb.ApiServices;

public class ApiKeyManagementService : IApiKeyManagementService
{
    private readonly Dbf25TeamArzContext _dbContext;

    public ApiKeyManagementService(Dbf25TeamArzContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool IsValidApiKey(string key)
    {
        var apiKey = _dbContext.ApiKeys.FirstOrDefault(k => k.Key == key && (k.ExpiredOn == null || k.ExpiredOn > DateTime.Now));
        return apiKey != null;
    }

    public async Task<string> AssignKeyAsync(int playerId)
    {
        string key = GenerateApiKey();

        var newApiKey = new ApiKey
        {
            Key = key,
            PlayerId = playerId,
            CreatedOn = DateTime.Now,
            ExpiredOn = DateTime.Now.AddDays(1)
        };

        _dbContext.ApiKeys.Add(newApiKey);
        await _dbContext.SaveChangesAsync();

        return key;
    }

    public async Task InvalidateApiKeyAsync(string key)
    {
        var apiKey = _dbContext.ApiKeys.FirstOrDefault(k => k.Key == key);
        if (apiKey != null)
        {
            apiKey.ExpiredOn = DateTime.Now;
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<ApiKeyDto>> GetPlayerKeysAsync(int playerId)
    {
        return await _dbContext.ApiKeys.Where(k => k.PlayerId == playerId).Select(k => new ApiKeyDto
        {
            Id = k.Id,
            Key = k.Key,
            CreatedOn = k.CreatedOn,
            ExpiredOn = k.ExpiredOn
        }).ToListAsync();
    }

    public static string GenerateApiKey(int length = 32)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }


}
