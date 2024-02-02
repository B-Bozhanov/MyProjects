namespace RealEstate.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IPaginationService
    {
        public PaginationModel CreatePagination(int itemsCount,int itemsPerPage, int currentPage, string controllerName, string actionName);

        public IEnumerable<T> Pager<T>(IEnumerable<T> TCollection, int currentPage);

        public IEnumerable<T> Pager<T, T2>(IQueryable<T2> TCollection, int currentPage);

        public IEnumerable<T> Pager<T, T2>(IOrderedQueryable<T2> TCollection, int currentPage);
    }
}
