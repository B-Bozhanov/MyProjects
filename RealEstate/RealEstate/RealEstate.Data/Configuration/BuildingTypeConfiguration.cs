namespace RealEstate.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RealEstate.Models.DataModels;

    public class BuildingTypeConfiguration : IEntityTypeConfiguration<BuildingType>
    {
        public void Configure(EntityTypeBuilder<BuildingType> builder)
        {
            builder.ToTable("BuildingTypes");
            builder.HasData
            (
                new BuildingType
                {
                    Id = 1,
                    Name = "Тухла"
                },
                new BuildingType
                {
                    Id = 2,
                    Name = "Панел"
                },
                new BuildingType
                {
                    Id = 3,
                    Name = "ЕПК"
                },
                 new BuildingType
                 {
                     Id = 4,
                     Name = "ПК"
                 },
                 new BuildingType
                 {
                     Id = 5,
                     Name = "Гредоред"
                 }
             );
        }
    }
}
