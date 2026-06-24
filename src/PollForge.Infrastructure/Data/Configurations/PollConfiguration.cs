using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollForge.Domain.Entities;

namespace PollForge.Infrastructure.Data.Configurations;

public class PollConfiguration : IEntityTypeConfiguration<Poll>
{
    public void Configure(EntityTypeBuilder<Poll> builder)
    {
        builder.ToTable("polls");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.Title)
            .HasColumnName("title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnName("description")
            .HasMaxLength(2000);

        builder.Property(p => p.AuthorId)
            .HasColumnName("author_id");

        builder.Property(p => p.Type)
            .HasColumnName("type");

        builder.Property(p => p.Status)
            .HasColumnName("status");

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(p => p.ClosesAt)
            .HasColumnName("closes_at");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Options)
            .WithOne()
            .HasForeignKey(o => o.PollId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Metadata.FindNavigation(nameof(Poll.Options))?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(p => p.AuthorId);
        builder.HasIndex(p => p.Status);
    }
}
