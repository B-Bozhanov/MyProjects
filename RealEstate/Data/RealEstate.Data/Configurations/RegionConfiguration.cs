namespace RealEstate.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RealEstate.Data.Models;

    public class RegionConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> region)
        {
            //region
            //   .HasOne(e => e.DownTown)
            //   .WithOne(e => e.Region)
            //   .HasForeignKey<Location>(e => e.DownTownId);
        }
    }
}
