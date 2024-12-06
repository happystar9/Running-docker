﻿using Microsoft.EntityFrameworkCore;
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

    }
}
