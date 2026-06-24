using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollForge.Domain.Entities;

namespace PollForge.Infrastructure.Data.Configurations;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.ToTable("votes");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasColumnName("id");

        builder.Property(v => v.PollId)
            .HasColumnName("poll_id");

        builder.Property(v => v.OptionId)
            .HasColumnName("option_id");

        builder.Property(v => v.VoterId)
            .HasColumnName("voter_id");

        builder.Property(v => v.Fingerprint)
            .HasColumnName("fingerprint")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(v => v.IpAddress)
            .HasColumnName("ip_address")
            .HasMaxLength(45)
            .IsRequired();

        builder.Property(v => v.CreatedAt)
            .HasColumnName("created_at");

        builder.HasIndex(v => new { v.PollId, v.Fingerprint })
            .IsUnique();

        builder.HasOne<Poll>()
            .WithMany()
            .HasForeignKey(v => v.PollId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<PollOption>()
            .WithMany()
            .HasForeignKey(v => v.OptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
