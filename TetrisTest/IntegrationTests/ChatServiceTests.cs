using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TetrisWeb.DTOs;
using Xunit.Abstractions;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.ApiServices;


namespace TetrisTest.IntegrationTests;


[Collection("SequentialTestExecution")]
public class ChatServiceTests : PostgresTestBase
{
    public ChatServiceTests(WebApplicationFactory<Program> webAppFactory, ITestOutputHelper outputHelper)
       : base(webAppFactory, outputHelper) { }

    [Fact]
    public async Task SendMessagePostsToChat()
    {
        var playerService = GetService<IPlayerService>();
        var chatService = GetService<IChatService>();

        var samplePlayer = new PlayerDto
        {
            Username = "Player32",
            Authid = "32",
            PlayerQuote = "TestQuote",
            AvatarUrl = "TestAvatarUrl",
            Isblocked = false
        };

        await playerService.CreatePlayerAsync(samplePlayer);
        var postedPlayer = await playerService.GetPlayerByAuthIdAsync("32");

        var message = new ChatDto
        {
            PlayerId = postedPlayer.Id,
            Message = "Test Message"
        };

        await chatService.PostChatAsync(message);

        var chatMessages = await chatService.GetRecentChatsAsync();

        chatMessages.Should().NotBeNullOrEmpty();
        chatMessages.Count.Should().Be(1);
        chatMessages.First().Message.Should().Be(message.Message);
        chatMessages.First().PlayerId.Should().Be(message.PlayerId);




    }

}
