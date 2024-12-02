using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TetrisWeb.GameData;

public partial class Dbf25TeamArzContext : DbContext
{
    public Dbf25TeamArzContext()
    {
    }

    public Dbf25TeamArzContext(DbContextOptions<Dbf25TeamArzContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApiKey> ApiKeys { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameSession> GameSessions { get; set; }

    public virtual DbSet<Leaderboard> Leaderboards { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=DB_CONN");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiKey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("api_key_pkey");

            entity.ToTable("api_key", "game");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedOn)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.ExpiredOn)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expired_on");
            entity.Property(e => e.Key)
                .HasMaxLength(256)
                .HasColumnName("key");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");

            entity.HasOne(d => d.Player).WithMany(p => p.ApiKeys)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("api_key_player_id_fkey");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chat_pkey");

            entity.ToTable("chat", "game");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Message)
                .HasMaxLength(150)
                .HasColumnName("message");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.TimeSent)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("time_sent");

            entity.HasOne(d => d.Player).WithMany(p => p.Chats)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chat_player_id_fkey");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_pkey");

            entity.ToTable("game", "game");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedByAuthId)
                .HasMaxLength(255)
                .HasColumnName("created_by_auth_id");
            entity.Property(e => e.PlayerCount).HasColumnName("player_count");
            entity.Property(e => e.StartTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_time");
            entity.Property(e => e.StopTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("stop_time");

            entity.HasOne(d => d.CreatedByAuth).WithMany(p => p.Games)
                .HasPrincipalKey(p => p.Authid)
                .HasForeignKey(d => d.CreatedByAuthId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_created_by_auth_id_fkey");
        });

        modelBuilder.Entity<GameSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_session_pkey");

            entity.ToTable("game_session", "game");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.Score).HasColumnName("score");

            entity.HasOne(d => d.Game).WithMany(p => p.GameSessions)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("game_session_game_id_fkey");

            entity.HasOne(d => d.Player).WithMany(p => p.GameSessions)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_session_player_id_fkey");
        });

        modelBuilder.Entity<Leaderboard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("leaderboard_pkey");

            entity.ToTable("leaderboard", "game");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.GamesPlayed).HasColumnName("games_played");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.TotalScore).HasColumnName("total_score");
            entity.Property(e => e.WinCount).HasColumnName("win_count");

            entity.HasOne(d => d.Player).WithMany(p => p.Leaderboards)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("leaderboard_player_id_fkey");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("player_pkey");

            entity.ToTable("player", "game");

            entity.HasIndex(e => e.Authid, "player_authid_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Authid)
                .HasMaxLength(450)
                .HasColumnName("authid");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
            entity.Property(e => e.Isblocked)
                .HasDefaultValue(false)
                .HasColumnName("isblocked");
            entity.Property(e => e.PlayerQuote)
                .HasMaxLength(80)
                .HasColumnName("player_quote");
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
