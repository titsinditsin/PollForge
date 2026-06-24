using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollForge.Domain.Entities;

namespace PollForge.Infrastructure.Data.Configurations;

public class PollOptionConfiguration : IEntityTypeConfiguration<PollOption>
{
    public void Configure(EntityTypeBuilder<PollOption> builder)
    {
        builder.ToTable("poll_options");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("id");

        builder.Property(o => o.PollId)
            .HasColumnName("poll_id");

        builder.Property(o => o.Text)
            .HasColumnName("text")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(o => o.DisplayOrder)
            .HasColumnName("display_order");

        builder.HasIndex(o => new { o.PollId, o.DisplayOrder });


    }
}
