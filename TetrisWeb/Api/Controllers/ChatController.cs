using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TetrisWeb.ApiServices;
using TetrisWeb.DTOs;

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

    //[HttpDelete]
    //public async Task<IActionResult> DeleteChat(int chatId)
    //{
    //    var success = await chatService.DeleteChatAsync(chatId);
        
    //    return Ok("Chat message deleted.");
    //}
}
