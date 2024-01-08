namespace RealEstate.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RealEstate.Services.Interfaces;

    public class PaginationService : IPaginationService
    {
        public PaginationModel CreatePagination(int itemsCount, int itemsPerPage, int currentPage, string controllerName, string actionName)
        {
            this.ValidateData(itemsCount, itemsPerPage, currentPage, controllerName, actionName);

            return new PaginationModel(itemsCount, itemsPerPage, currentPage, controllerName, actionName);
        }

        public IEnumerable<T> Pager<T>(IEnumerable<T> TCollection, int currentPage, int itemsPerPage)
            => TCollection
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage);

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
