using Microsoft.AspNetCore.Mvc;
using TetrisShared.DTOs;

namespace TetrisWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController(IChatService chatService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostChat([FromBody] ChatDto chatDto)
    {
        var newChat = await chatService.PostChatAsync(chatDto);
        return Results.Ok(newChat);
    }

    [HttpGet]
    public async Task<IResult> GetRecentChats()
    {
        return Results.Ok(await chatService.GetRecentChatsAsync());
    }
}
