namespace RealEstate.Services
{
    public class PaginationModel
    {
        public PaginationModel(int itemsCount, int itemsPerPage, int currentPage, string controllerName, string actionName)
        {
            this.LastPage = itemsCount / itemsPerPage;

            if (itemsCount % itemsPerPage != 0)
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

            this.ControllerName = controllerName;
            this.ActionName = actionName;
        }

        public int FirstPage { get; set; } = 1;

        public int PreviousPage => this.CurrentPage - 1;

        public int CurrentPage { get; set; }

        public int NextPage => this.CurrentPage + 1;

        public int LastPage { get; set; }

        public string ControllerName { get; init; }

        public string ActionName { get; init; }
    }
}
