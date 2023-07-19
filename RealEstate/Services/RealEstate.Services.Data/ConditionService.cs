namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.ConditionModel;

    public class ConditionService : IConditionService
    {
        private readonly IDeletableEntityRepository<Condition> conditionRepository;

        public ConditionService(IDeletableEntityRepository<Condition> conditionRepository)
        => this.conditionRepository = conditionRepository;
        
        public async Task<IList<ConditionViewModel>> GetAllAsync()
            => await this.conditionRepository
            .All()
            .To<ConditionViewModel>()
            .ToListAsync();
    }
}
