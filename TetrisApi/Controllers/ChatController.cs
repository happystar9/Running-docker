using Microsoft.AspNetCore.Mvc;
using TetrisShared.DTOs;

namespace TetrisApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController(IChatService chatService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> RegisterPlayer([FromBody] ChatDto chatDto)
    {
        var newChat = await chatService.PostChatAsync(chatDto);
        return Results.Ok(newChat);
    }
}
