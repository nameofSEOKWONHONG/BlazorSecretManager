using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorSecretManager.Entities;

public class Notification
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Extra { get; set; }
    public bool IsRead { get; set; }
    public DateTime PublishDate { get; set; }
}

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.UserId)
            .HasMaxLength(100);
        builder.Property(x => x.Type).IsRequired()
            .HasMaxLength(2);
        builder.Property(x => x.Title).IsRequired()
            .HasMaxLength(200);
        builder.Property(x => x.Content).IsRequired()
            .HasMaxLength(4000);
        builder.Property(x => x.Extra)
            .HasMaxLength(1000);
        builder.Property(x => x.IsRead).HasDefaultValue(false);
        builder.Property(x => x.PublishDate).IsRequired();
    }
}