namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.EquipmentModel;

    public class EquipmentService : IEquipmentService
    {
        private readonly IDeletableEntityRepository<Equipment> equipmentRepository;

        public EquipmentService(IDeletableEntityRepository<Equipment> equipmentRepository)
            => this.equipmentRepository = equipmentRepository;

        public async Task<IList<EquipmentViewModel>> GetAllAsync()
            => await this.equipmentRepository
            .All()
            .To<EquipmentViewModel>()
            .ToListAsync();
    }
}
