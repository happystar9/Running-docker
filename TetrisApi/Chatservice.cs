using Microsoft.EntityFrameworkCore;
using TetrisApi.Data;

namespace TetrisApi
{
	public class ChatService(Dbf25TeamArzContext context)
	{
		public async Task<ICollection<Chat>> GetChatAsync()
		{
			return await context.Chats.ToListAsync();
		}

		public async Task<IResult> PostChatAsync(Chat chat)
		{
			//not fully implemented in endpoint
			context.Add(chat);
			await context.SaveChangesAsync();
			return Results.Created();
		}
	}
}
