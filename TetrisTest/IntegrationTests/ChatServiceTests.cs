using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisWeb.ApiServices.Interfaces;
using Xunit.Abstractions;
using TetrisWeb.DTOs;
using FluentAssertions;

namespace TetrisTest.IntegrationTests;


[Collection("SequentialTestExecution")]
public class ChatServiceTests : PostgresTestBase
{
    public ChatServiceTests(WebApplicationFactory<Program> webAppFactory, ITestOutputHelper outputHelper)
       : base(webAppFactory, outputHelper) { }


    public async Task SendMessagePostsToChat()
    {
        var playerService = GetService<IPlayerService>();
        var chatService = GetService<IChatService>();

        var samplePlayer = new PlayerDto
        {
            Username = "Player12",
            Authid = "12",
            PlayerQuote = "TestQuote",
            AvatarUrl = "TestAvatarUrl",
            Isblocked = false
        };

        await playerService.CreatePlayerAsync(samplePlayer);
        var postedPlayer = await playerService.GetPlayerByAuthIdAsync("12");

        var message = new ChatDto
        {
            PlayerId = postedPlayer.Id,
            Message = "Test Message"
        };

        await chatService.PostChatAsync(message);

        var chatMessages = await chatService.GetRecentChatsAsync();

        chatMessages.Should().NotBeNullOrEmpty();
        chatMessages.Count.Should().Be(1);
        chatMessages.Should().ContainEquivalentOf(message);




    }

}
