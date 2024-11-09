using TetrisShared.DTOs;

public interface IChatService
{
    Task<List<ChatDto>> GetAllChatsAsync();
    Task<ChatDto> PostChatAsync(ChatDto chatDto);
}