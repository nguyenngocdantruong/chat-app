using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Data;

public partial class AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
    : DbContext(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public virtual DbSet<AuditLog> AuditLogs { get; set; }
    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<ConversationEvent> ConversationEvents { get; set; }

    public virtual DbSet<ConversationMember> ConversationMembers { get; set; }

    public virtual DbSet<ConversationSetting> ConversationSettings { get; set; }

    public virtual DbSet<ConversationStatus> ConversationStatuses { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MessageStatus> MessageStatuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSetting> UserSettings { get; set; }

    public virtual DbSet<FcmToken> FcmTokens { get; set; } = null!;

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PK__AuditLog__3214EC074AADNS3BC3");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PK__Attachme__3214EC074ACF3BC3");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PK__Conversa__3214EC0766D87C8B");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsGroup).HasDefaultValue(false);
        });

        modelBuilder.Entity<ConversationEvent>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PK__ConversatEv__3214EC07B9006160");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Type).HasConversion<string>().HasMaxLength(30);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ConversationMember>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PK__Conversa__3214EC0763BEF676");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.JoinedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Role).HasDefaultValue(UserRole.Member).HasConversion<string>();
        });

        modelBuilder.Entity<ConversationSetting>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PK__Conversa__3214EC07B9006160");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.MuteNotification).HasDefaultValue(false);
            entity.Property(e => e.Pinned).HasDefaultValue(false);
        });

        modelBuilder.Entity<ConversationStatus>(entity => 
        {
            entity.HasKey(e => e.Guid).HasName("PK__ConversatStt__3214EC07B9006160");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PK__Friends__3214EC0751635760");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(FriendStatus.Unknown).HasConversion<string>().HasMaxLength(20);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PK__Messages__3214EC0768CCBD84");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.MessageType).HasDefaultValue(MessageType.Unknown).HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.IsEdited).HasDefaultValue(false);
            entity.Property(e => e.SentAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<MessageStatus>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PK__MessageS__3214EC0729D84059");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Reaction).HasDefaultValue(Reaction.None).HasConversion<string>().HasMaxLength(5);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PK__Users__3214EC07032C0070");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsOnline).HasDefaultValue(false);
            entity.Property(e => e.IsSearchable).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PK__UserSett__3214EC07D844488A");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.EnableE2e).HasDefaultValue(false);
            entity.Property(e => e.MuteAllNotifications).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<FcmToken>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PK__FcmToken__3214EC07B1");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Token).IsRequired().HasMaxLength(4000);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    private Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}
