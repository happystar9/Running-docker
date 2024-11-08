using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
using TetrisWeb.DTOs;
using TetrisWeb.GameData;
using TetrisWeb.ApiServices;
using System.Numerics;
using Microsoft.EntityFrameworkCore;

namespace TetrisTest
{
	public class IntegrationTests
	{
		//[Fact]
		//public async Task PlayerRegistration()
		//{
		//	var playerService = new PlayerService();


		//	var newPlayer = new PlayerDto
		//	{
		//		Authid = "TestAuth123",
		//		PlayerQuote = "Just testing!",
		//		AvatarUrl = "http://example.com/avatar.jpg",
		//		ApiKey = "TestApiKey123",
		//		Isblocked = false
		//	};


		//	var createdPlayer = await playerService.CreatePlayerAsync(newPlayer);


		//	Assert.NotNull(createdPlayer);
		//	Assert.Equal("TestAuth123", createdPlayer.Authid);
		//	Assert.Equal("Just testing!", createdPlayer.PlayerQuote);
		//	Assert.Equal("http://example.com/avatar.jpg", createdPlayer.AvatarUrl);
		//	Assert.Equal("TestApiKey123", createdPlayer.ApiKey);
		//	Assert.False(createdPlayer.Isblocked);


		//	var playerInDb = await context.Players.FindAsync(createdPlayer.Id);
		//	Assert.NotNull(playerInDb);
		//	Assert.Equal("TestAuth123", playerInDb.Authid);

		//}

		[Fact]
		public void GameCreation()
		{
			throw new NotImplementedException();
		}
	}
}