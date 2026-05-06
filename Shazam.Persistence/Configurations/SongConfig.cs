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
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.YoutubeUrl).IsRequired();
            builder.Property(x => x.ThumbnailUrl);
        }
    }
}
