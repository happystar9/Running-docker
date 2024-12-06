using Microsoft.EntityFrameworkCore;
using TetrisWeb.DTOs;
using TetrisWeb.GameData;

namespace TetrisWeb.ApiServices
{
    public class ChatService(Dbf25TeamArzContext dbcontext) : IChatService
    {
        public Action OnMessage { get; set; }

        public async Task<List<ChatDto>> GetAllChatsAsync()
        {
            var chats = await dbcontext.Chats
            .Join(dbcontext.Players,
            c => c.PlayerId,
            p => p.Id,
            (c, p) => new ChatDto
            {
                PlayerId = c.PlayerId,
                PlayerUsername = p.Username,
                Message = c.Message,
                TimeSent = c.TimeSent
            })
            .ToListAsync();

            return chats;
        }

        public async Task<List<ChatDto>> GetRecentChatsAsync()
        {
            var chats = await dbcontext.Chats
                .OrderByDescending(c => c.TimeSent)
                .Take(20)
                .Join(dbcontext.Players,
                    c => c.PlayerId,
                    p => p.Id,
                    (c, p) => new ChatDto
                    {
                        PlayerId = c.PlayerId,
                        PlayerUsername = p.Username,
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

            OnMessage?.Invoke();
            return result;
        }

        public async Task<IResult> DeleteChatAsync(int chatId)
        {
            try
            {
                // Find the chat to delete
                var chatToDelete = await dbcontext.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
                if (chatToDelete == null)
                {
                    return Results.NotFound("Chat not found.");
                }

                // Remove the chat from the database
                dbcontext.Chats.Remove(chatToDelete);
                await dbcontext.SaveChangesAsync();

                // Notify listeners of the deletion
                OnMessage?.Invoke();

                return Results.Ok("Chat deleted successfully.");
            }
            catch (Exception ex)
            {
                // Handle any errors
                return Results.Problem($"An error occurred: {ex.Message}");
            }
        }
    }
}
