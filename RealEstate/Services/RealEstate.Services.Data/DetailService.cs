namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.DetailModel;

    public class DetailService : IDetailService
    {
        private readonly IDeletableEntityRepository<Detail> detailRepository;

        public DetailService(IDeletableEntityRepository<Detail> detailRepository)
            => this.detailRepository = detailRepository;

        public async Task<IList<DetailViewModel>> GetAllAsync()
            => await this.detailRepository
            .All()
            .To<DetailViewModel>()
            .ToListAsync();
    }
}
