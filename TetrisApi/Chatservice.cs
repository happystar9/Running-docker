using TetrisApi.Data;

namespace TetrisApi
{
	public class ChatService : IChatservice
	{
		private readonly HttpClient _httpClient;

		public ChatService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<List<Chat>> GetChatAsync()
		{
			// Get all chats, can add where to specify which date
			var response = await _httpClient.GetFromJsonAsync<List<Chat>>("select * from dbf25_team_arz.game.chat");
			return response;
		}
	}
}
