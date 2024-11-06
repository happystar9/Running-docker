using Microsoft.EntityFrameworkCore;
using TetrisApi.Data;
using TetrisShared.DTOs;

namespace TetrisApi.Services
{
    public class ChatService(Dbf25TeamArzContext dbcontext) : IChatService
    {

        public async Task<List<ChatDto>> GetAllChatsAsync()
        {
            var chats = await dbcontext.Chats
            .Select(c => new ChatDto
            {
                PlayerId = c.PlayerId,
                Message = c.Message,
                TimeSent = c.TimeSent
            })
            .ToListAsync();

            return chats;
        }

        public async Task<ChatDto> PostChatAsync(ChatDto chatDto)
        {
            Chat chatObj = new Chat
            {
                Id = chatDto.Id,
                PlayerId = chatDto.PlayerId,
                Message = chatDto.Message,
                TimeSent = DateTime.Now
            };
            await dbcontext.Chats.AddAsync(chatObj);
            await dbcontext.SaveChangesAsync();

            var result = new ChatDto
            {
                Id = chatDto.Id,
                PlayerId = chatDto.PlayerId,
                Message = chatDto.Message,
                TimeSent = chatObj.TimeSent
            };

            return result;
        }
        
    }
}
public interface IChatService
{
    Task<List<ChatDto>> GetAllChatsAsync();
    Task<ChatDto> PostChatAsync(ChatDto chatDto);
}