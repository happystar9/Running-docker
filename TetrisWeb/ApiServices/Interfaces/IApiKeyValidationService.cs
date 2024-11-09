using TetrisWeb.DTOs;

namespace TetrisWeb.ApiServices.Interfaces;

public interface IApiKeyValidationService
{
    bool IsValidApiKey(string key);
    Task InvalidateApiKeyAsync(string key);
    Task AssignKeyAsync(PlayerDto player, string key);
}
