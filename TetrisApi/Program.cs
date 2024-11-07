using Microsoft.EntityFrameworkCore;
using TetrisWeb.AuthData;
using TetrisWeb.ApiServices;
using TetrisShared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration["DB_CONN"];


builder.Services.AddDbContext<Dbf25TeamArzContext>(options => options.UseNpgsql(connString));
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IChatService, ChatService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//not fully implemented
//app.MapPost("/chats", (ChatService chat) => ChatService.GetChatAsync());
//app.MapGet("/chats", (ChatService chat) => ChatService.PostChatAsync());

app.Run();
