using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorSecretManager.Entities;

public class ChatRoom
{
    public int Id { get; set; }
    public string OwnerId { get; set; }
    public string Name { get; set; }
    public string AttendUsers { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<Chat> Chats { get; set; }
}

public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.ToTable("ChatRooms");
        builder.HasKey(x => x.Id);
        builder.Property(x =>x .Id).ValueGeneratedOnAdd();
        builder.Property(x => x.AttendUsers)
            .HasMaxLength(4000)
            .IsRequired();
        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.HasMany(m => m.Chats)
            .WithOne(x => x.ChatRoom)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class Chat
{
    public int Id { get; set; }
    public string FromUserId { get; set; }
    public string FromUserName { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; }
}

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable("Chats");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.FromUserId)
            .HasMaxLength(36)
            .IsRequired();
        builder.Property(x => x.FromUserName)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x => x.Message)
            .HasMaxLength(8000)
            .IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.HasOne(x => x.ChatRoom)
            .WithMany(x => x.Chats)
            .HasForeignKey(x => x.ChatRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}