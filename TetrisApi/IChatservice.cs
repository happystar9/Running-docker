using TetrisApi.Data;

namespace TetrisApi
{
	public interface IChatservice
	{
		Task<List<Chat>> GetChatAsync();
	}
}
