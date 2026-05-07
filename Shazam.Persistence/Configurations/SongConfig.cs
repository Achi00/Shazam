using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shazam.Domain.Entity;

namespace Shazam.Persistence.Configurations
{
    public class SongConfig : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.ToTable("Songs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Author)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.YoutubeUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ThumbnailUrl);

            // converting entity class type from TimeSpan to int
            builder.Property(x => x.Duration)
                .HasConversion(
                    t => t.HasValue ? (int?)t.Value.TotalSeconds : null,
                    i => i.HasValue ? TimeSpan.FromSeconds(i.Value) : null
                );

            // added url index
            builder.HasIndex(x => x.YoutubeUrl).IsUnique();
        }
    }
}
