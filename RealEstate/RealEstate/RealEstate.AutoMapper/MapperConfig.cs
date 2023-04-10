using AutoMapper;

namespace RealEstate.AutoMapper
{
    public class MapperConfig
    {
        public static Mapper Map<TSourse, TDestination>() where TSourse : class where TDestination : class
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSourse, TDestination>();
            });

            var mapper = new Mapper(config);

            return mapper;
        }
    }
}
