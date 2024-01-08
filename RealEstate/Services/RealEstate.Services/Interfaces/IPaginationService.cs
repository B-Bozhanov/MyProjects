namespace RealEstate.Services.Interfaces
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IPaginationService
    {
        public PaginationModel CreatePagination(int itemsCount,int itemsPerPage, int currentPage, string controllerName, string actionName);

        public IEnumerable<T> Pager<T>(IEnumerable<T> TCollection, int currentPage, int itemsPerPage);
    }
}
