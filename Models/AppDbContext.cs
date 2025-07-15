using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<ConversationMember> ConversationMembers { get; set; }

    public virtual DbSet<ConversationSetting> ConversationSettings { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MessageStatus> MessageStatuses { get; set; }

    public virtual DbSet<OtpRequest> OtpRequests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSetting> UserSettings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attachme__3214EC074ACF3BC3");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Message).WithMany(p => p.Attachments).HasConstraintName("FK__Attachmen__Messa__76969D2E");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conversa__3214EC0766D87C8B");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsGroup).HasDefaultValue(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Conversations).HasConstraintName("FK__Conversat__Creat__4E88ABD4");
        });

        modelBuilder.Entity<ConversationMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conversa__3214EC0763BEF676");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.JoinedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Role).HasDefaultValue("member");

            entity.HasOne(d => d.Conversation).WithMany(p => p.ConversationMembers).HasConstraintName("FK__Conversat__Conve__534D60F1");

            entity.HasOne(d => d.User).WithMany(p => p.ConversationMembers).HasConstraintName("FK__Conversat__UserI__5441852A");
        });

        modelBuilder.Entity<ConversationSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conversa__3214EC07B9006160");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.MuteNotification).HasDefaultValue(false);
            entity.Property(e => e.Pinned).HasDefaultValue(false);

            entity.HasOne(d => d.Conversation).WithMany(p => p.ConversationSettings).HasConstraintName("FK__Conversat__Conve__70DDC3D8");

            entity.HasOne(d => d.User).WithMany(p => p.ConversationSettings).HasConstraintName("FK__Conversat__UserI__6FE99F9F");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Friends__3214EC0751635760");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Addressee).WithMany(p => p.FriendAddressees).HasConstraintName("FK__Friends__Address__47DBAE45");

            entity.HasOne(d => d.Requester).WithMany(p => p.FriendRequesters).HasConstraintName("FK__Friends__Request__46E78A0C");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messages__3214EC0768CCBD84");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsEdited).HasDefaultValue(false);
            entity.Property(e => e.MessageType).HasDefaultValue("text");
            entity.Property(e => e.SentAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages).HasConstraintName("FK__Messages__Conver__59FA5E80");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages).HasConstraintName("FK__Messages__Sender__5AEE82B9");
        });

        modelBuilder.Entity<MessageStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MessageS__3214EC0729D84059");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsRead).HasDefaultValue(false);

            entity.HasOne(d => d.Message).WithMany(p => p.MessageStatuses).HasConstraintName("FK__MessageSt__Messa__6383C8BA");

            entity.HasOne(d => d.User).WithMany(p => p.MessageStatuses).HasConstraintName("FK__MessageSt__UserI__6477ECF3");
        });

        modelBuilder.Entity<OtpRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OtpReque__3214EC07E0089FF1");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsUsed).HasDefaultValue(false);

            entity.HasOne(d => d.User).WithMany(p => p.OtpRequests).HasConstraintName("FK__OtpReques__UserI__412EB0B6");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07032C0070");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsOnline).HasDefaultValue(false);
            entity.Property(e => e.IsSearchable).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserSett__3214EC07D844488A");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.EnableE2e).HasDefaultValue(false);
            entity.Property(e => e.MuteAllNotifications).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.UserSettings).HasConstraintName("FK__UserSetti__UserI__693CA210");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
