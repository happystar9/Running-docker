using TetrisWeb.DTOs;

namespace TetrisWeb.ApiServices.Interfaces;

public interface IApiKeyManagementService
{
    bool IsValidApiKey(string key);
    Task InvalidateApiKeyAsync(string key);
    Task<string> AssignKeyAsync(int playerId);
    Task<List<ApiKeyDto>> GetPlayerKeysAsync(int playerId);
}
