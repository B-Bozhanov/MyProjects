namespace RealEstate.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RealEstate.Data.Models;

    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> region)
        {
            region
               .HasOne(e => e.DownTown)
               .WithOne(e => e.Region)
               .HasForeignKey<Region>(e => e.DownTownId);
        }
    }
}
