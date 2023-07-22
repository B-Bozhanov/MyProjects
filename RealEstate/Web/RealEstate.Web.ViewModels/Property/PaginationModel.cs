namespace RealEstate.Web.ViewModels.Property
{
    using static RealEstate.Common.GlobalConstants.Properties;
    public class PaginationModel
    {
        public PaginationModel(int allPropertiesCount, int currentPage)
        {
           this.LastPage = allPropertiesCount / PropertiesPerPage;

            if (allPropertiesCount % PropertiesPerPage != 0)
            {
                this.LastPage++;
            }

            if (currentPage > this.LastPage)
            {
               this.CurrentPage = this.LastPage;
            }
            else if (currentPage < 1)
            {
                this.CurrentPage = 1;
            }
            else
            {
                this.CurrentPage = currentPage;
            }
        }

        public int PreviousPage => this.CurrentPage - 1; 

        public int CurrentPage { get; set; }

        public int NextPage => this.CurrentPage + 1;

        public int LastPage { get; set; }
    }
}
