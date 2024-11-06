using Microsoft.EntityFrameworkCore;
using TetrisApi.Data;
using TetrisShared.DTOs;

namespace TetrisApi.Services
{
    public class ChatService(Dbf25TeamArzContext dbcontext) : IChatService
    {

        public Task<ChatDto> GetChatAsync(Chat chat)
        {
            throw new NotImplementedException();
        }

        public Task<ChatDto> PostChatAsync(Chat chat)
        {
            throw new NotImplementedException();
        }

        
    }
}
public interface IChatService
{
    Task<ChatDto> GetChatAsync(Chat chat);
    Task<ChatDto> PostChatAsync(Chat chat);
}