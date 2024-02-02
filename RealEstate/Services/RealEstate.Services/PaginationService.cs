namespace RealEstate.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RealEstate.Common;
    using RealEstate.Services.Interfaces;
    using RealEstate.Services.Mapping;

    public class PaginationService : IPaginationService
    {
        public PaginationModel CreatePagination(int itemsCount, int itemsPerPage, int currentPage, string controllerName, string actionName)
        {
            this.ValidateData(itemsCount, itemsPerPage, currentPage, controllerName, actionName);

            return new PaginationModel(itemsCount, itemsPerPage, currentPage, controllerName, actionName);
        }

        public IEnumerable<T> Pager<T>(IEnumerable<T> TCollection, int currentPage)
            => TCollection
                .Skip((currentPage - 1) * GlobalConstants.Properties.PropertiesPerPage)
                .Take(GlobalConstants.Properties.PropertiesPerPage);

        public IEnumerable<T> Pager<T, T2>(IQueryable<T2> TCollection, int currentPage)
            => TCollection
                .Skip((currentPage - 1) * GlobalConstants.Properties.PropertiesPerPage)
                .Take(GlobalConstants.Properties.PropertiesPerPage)
                .To<T>()
                .ToList();
        public IEnumerable<T> Pager<T, T2>(IOrderedQueryable<T2> TCollection, int currentPage)
           => TCollection
               .Skip((currentPage - 1) * GlobalConstants.Properties.PropertiesPerPage)
               .Take(GlobalConstants.Properties.PropertiesPerPage)
               .To<T>()
               .ToList();

        private void ValidateData(int itemsCount, int itemsPerPage, int currentPage, string controllerName, string actionName)
        {
            if (itemsCount < 0)
            {
                throw new InvalidOperationException("Items can not be negative");
            }
            else if (itemsPerPage <= 0 || itemsPerPage > 100)
            {
                throw new InvalidOperationException("Items must be between 1 and 100");
            }
            else if (currentPage <= 0)
            {
                throw new InvalidOperationException("Current page can not be negative");
            }
            else if (controllerName == null || actionName == null)
            {
                throw new InvalidOperationException("Controller name and action name canot be null");
            }
        }
    }
}
