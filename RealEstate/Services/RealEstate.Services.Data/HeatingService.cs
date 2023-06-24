namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.HeatingModel;

    public class HeatingService : IHeatingService
    {
        private readonly IDeletableEntityRepository<Heating> heatingRepository;

        public HeatingService(IDeletableEntityRepository<Heating> heatingRepository)
            => this.heatingRepository = heatingRepository;

        public async Task<IList<HeatingViewModel>> GetAllAsync()
            => await this.heatingRepository
            .All()
            .To<HeatingViewModel>()
            .ToListAsync();
    }
}
