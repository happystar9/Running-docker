using TetrisShared.DTOs;

public interface IChatService
{
    Task<List<ChatDto>> GetAllChatsAsync();
    Task<List<ChatDto>> GetRecentChatsAsync();
    Task<ChatDto> PostChatAsync(ChatDto chatDto);
    public Action OnMessage { get; set; }

}