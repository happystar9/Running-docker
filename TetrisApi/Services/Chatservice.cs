using Microsoft.EntityFrameworkCore;
using TetrisApi.Data;
using TetrisShared.DTOs;

namespace TetrisApi.Services
{
    public class ChatService(Dbf25TeamArzContext dbcontext) : IChatService
    {

        public async Task<List<ChatDto>> GetAllChatsAsync(Chat chat)
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

        public async Task<IResult> PostChatAsync(Chat chat)
        {
            await dbcontext.Chats.AddAsync(chat);
            await dbcontext.SaveChangesAsync();
            return Results.Created();
        }
        
    }
}
public interface IChatService
{
    Task<List<ChatDto>> GetAllChatsAsync(Chat chat);
    Task<IResult> PostChatAsync(Chat chat);
}