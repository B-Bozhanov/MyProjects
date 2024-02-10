namespace RealEstate.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.Infrastructure.Extencions;
    using RealEstate.Web.ViewModels.Search;

    public class PropertyGetService : IPropertyGetService
    {
        private readonly IDeletableEntityRepository<Property> propertyRepository;

        public PropertyGetService(IDeletableEntityRepository<Property> propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public int GetAllExpiredPropertiesCount()
        {
            return this.propertyRepository
                .All()
                .Where(p => p.IsExpired)
                .ToList()
                .Count;
        }

        public async Task<T> GetByIdAsync<T>(int id)
            => await this.GetAllWithoutExpired()
                 .Where(p => p.Id == id)
                 .To<T>()
                 .FirstOrDefaultAsync();

        public async Task<T> GetByIdWithExpiredUserPropertiesAsync<T>(int id, string userId)
            => await this.propertyRepository
                 .All()
                 .Where(p => p.Id == id && p.ApplicationUserId == userId)
                 .To<T>()
                 .FirstOrDefaultAsync();

        public IEnumerable<T> GetTopNewest<T>(int count)
            => this.GetAllWithoutExpired()
                 .OrderByDescending(p => p.Id)
                 .Take(count)
                 .To<T>()
                 .ToArray();

        public IEnumerable<T> GetTopMostExpensive<T>(int count)
            => this.GetAllWithoutExpired()
                 .OrderByDescending(p => p.Price)
                 .Take(count)
                 .To<T>()
                 .ToArray();

        public int GetAllActiveCount()
            => this.GetAllWithoutExpired()
                  .ToArray()
                  .Length;

        public int GetAllActiveUserPropertiesCount(string userId)
            => this.GetAllWithoutExpired()
            .Where(p => p.ApplicationUserId == userId)
            .OrderByDescending(p => p.Id)
            .ToArray()
            .Length;

        public int GetAllExpiredUserPropertiesCount(string userId)
            => this.propertyRepository
            .All()
            .Where(p => p.ApplicationUserId == userId && p.IsExpired)
            .OrderByDescending(p => p.Id)
            .ToArray()
            .Length;

        //TODO: Remove this!!!
        public async Task<Property> GetByIdAsync(int id)
            => await this.propertyRepository
            .All()
            .FirstOrDefaultAsync(p => p.Id == id);

        public IEnumerable<T> GetAllByOptionIdPerPage<T>(int optionId, int page)
        {
            var result = optionId switch
            {
                (int)OptionType.NewToOld => this.GetAllWithoutExpired().OrderByDescending(p => p.Id).To<T>().ToList(),
                (int)OptionType.OldToNew => this.GetAllWithoutExpired().OrderBy(p => p.Id).To<T>().ToList(),
                (int)OptionType.ForSale => this.GetAllWithoutExpired().Where(p => p.Option == PropertyOption.Sale).OrderByDescending(p => p.Id).To<T>().ToList(),
                (int)OptionType.ForRent => this.GetAllWithoutExpired().Where(p => p.Option == PropertyOption.Rent).OrderByDescending(p => p.Id).To<T>().ToList(),
                (int)OptionType.PriceDesc => this.GetAllWithoutExpired().OrderByDescending(p => p.Price).To<T>().ToList(),
                (int)OptionType.PriceAsc => this.GetAllWithoutExpired().OrderBy(p => p.Price).To<T>().ToList(),
                (int)OptionType.Test => throw new InvalidOperationException("TEST, test"),
                _ => this.GetAllWithoutExpired().OrderByDescending(p => p.Id).To<T>().ToList(),
            };

            if (result.IsNullOrEmpty())
            {
                return null;                //throw new ArgumentException("The databse properties is Empty");
            }

            var pagedProperties = result.GetPagination<T>(page);
            return pagedProperties;
        }

        public async Task<IEnumerable<T>> GetActiveUserPropertiesPerPageAsync<T>(string id, int page)
        {
            var activeProperties = await this.GetAllWithoutExpired()
                 .Where(p => p.ApplicationUserId == id)
                 .OrderByDescending(p => p.Id)
                 .To<T>()
                 .ToListAsync();

            var pagedProperties = activeProperties.GetPagination<T>(page);
            return pagedProperties;
        }

        public async Task<IEnumerable<T>> GetExpiredUserPropertiesPerPageAsync<T>(string id, int page)
        {
            var expiredProperties = await this.propertyRepository
                 .All()
                 .Where(p => p.ApplicationUserId == id && p.IsExpired)
                 .OrderByDescending(p => p.Id)
                 .To<T>()
                 .ToListAsync();

            return expiredProperties.GetPagination<T>(page);
        }

        public async Task<IEnumerable<T>> GetAllWithExpiredUserPropertiesPerPage<T>(string id, int page)
        {
            var allProperties = await this.propertyRepository
                 .All()
                 .Where(p => p.ApplicationUserId == id)
                 .OrderByDescending(p => p.DeletedOn)
                 .To<T>()
                 .ToListAsync();

            var pagedProperties = allProperties.GetPagination<T>(page);
            return pagedProperties;
        }

        private IQueryable<Property> GetAllWithoutExpired()
                => this.propertyRepository
                       .AllAsNoTracking()
                       .Where(x => !x.IsExpired);
    }
}
