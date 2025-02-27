﻿using TetrisWeb.DTOs;

public interface IChatService
{
    Task<List<ChatDto>> GetAllChatsAsync();
    Task<List<ChatDto>> GetRecentChatsAsync();
    Task<ChatDto> PostChatAsync(ChatDto chatDto);
    Task DeleteChatAsync(int chatId);
    public Action OnMessage { get; set; }

}