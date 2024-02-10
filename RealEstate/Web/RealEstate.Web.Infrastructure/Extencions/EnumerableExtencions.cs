namespace RealEstate.Web.Infrastructure.Extencions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RealEstate.Common;
    public static class EnumerableExtencions
    {
        public static IEnumerable<T> GetPagination<T>(this IEnumerable<T> TCollection, int currentPage) where T : notnull
        {
            if (TCollection == null)
            {
                throw new ArgumentNullException("Collection can not be null");
            }

            return TCollection
               .Skip((currentPage - 1) * GlobalConstants.Properties.PropertiesPerPage)
               .Take(GlobalConstants.Properties.PropertiesPerPage);
        }
    }
}
