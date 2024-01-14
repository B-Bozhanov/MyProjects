namespace RealEstate.Services.TranslateService.Models
{
    using System.Collections.Generic;
    //TODO: May be move languages files in GlobalConstants
    //TODO: Group the properties in classes
    public abstract class LanguageBase
    {
        public LanguageBase()
        {
            this.Dictionary = new Dictionary<string, string>
            {
                {nameof(NewestProperties), this.NewestProperties},
                {nameof(MostExpensiveProperties), this.MostExpensiveProperties},
                {nameof(AllProperties), this.AllProperties},
                {nameof(HomePage), this.HomePage},
                {nameof(About), this.About},
                {nameof(Properties), this.Properties},
                {nameof(Blog), this.Blog},
                {nameof(Pages), this.Pages},
                {nameof(Contact), this.Contact},
                {nameof(Register), this.Register},
                {nameof(Login), this.Login},
                {nameof(MyActiveProperties), this.MyActiveProperties},
                {nameof(IamAAgent), this.IamAAgent},
                {nameof(Admin), this.Admin},
                {nameof(Profile), this.Profile},
                {nameof(Add), this.Add},
                {nameof(Search), this.Search},
                {nameof(Title), this.Title},
                {nameof(Type), this.Type},
                {nameof(TotalPropertiesCount), this.TotalPropertiesCount},
                {nameof(Location), this.Location},
                {nameof(DetailInfo), this.DetailInfo},
                {nameof(Size), this.Size},
                {nameof(Year), this.Year},
                {nameof(Floor), this.Floor},
                {nameof(TotalFloors), this.TotalFloors},
                {nameof(BedRooms), this.BedRooms},
                {nameof(BathRooms), this.BathRooms},
                {nameof(Garages), this.Garages},
                {nameof(BuildingType), this.BuildingType},
                {nameof(Condition), this.Condition},
                {nameof(Equipments), this.Equipments},
                {nameof(Details), this.Details},
                {nameof(Description), this.Description},
                {nameof(Options), this.Options},
                {nameof(Price), this.Price},
                {nameof(Expiration), this.Expiration},
                {nameof(IncreaseExpiration), this.IncreaseExpiration},
                {nameof(Contacts), this.Contacts},
                {nameof(Summary), this.Summary},
                {nameof(Id), this.Id},
                {nameof(Amenities), this.Amenities},
                {nameof(ContactAgent), this.ContactAgent},
                {nameof(PropertyType), this.PropertyType},
                {nameof(ExpirationDays), this.ExpirationDays},
                {nameof(ContactModel), this.ContactModel},
                {nameof(SelectLocation), this.SelectLocation},
                {nameof(SelectPopulatedPlace), this.SelectPopulatedPlace},
                {nameof(PopulatedPlace), this.PopulatedPlace},
                {nameof(TotalBedRooms), this.TotalBedRooms},
                {nameof(TotalBathRooms), this.TotalBathRooms},
                {nameof(TotalGarages), this.TotalGarages},
                {nameof(Heating), this.Heating},
                {nameof(MoreInfo), this.MoreInfo},
                {nameof(Sale), this.Sale},
                {nameof(Rent), this.Rent},
                {nameof(PriceInEUR), this.PriceInEUR},
                {nameof(ChooseImage), this.ChooseImage},
                {nameof(Username), this.Username},
                {nameof(Password), this.Password},
                {nameof(YouHaveNotAddedProperties), this.YouHaveNotAddedProperties},
                {nameof(YouHaveNotUnactiveProperties), this.YouHaveNotUnactiveProperties},
                {nameof(ExpiredProperties), this.ExpiredProperties},
                {nameof(ForgotPassword), this.ForgotPassword},
                {nameof(CreateAccount), this.CreateAccount},
            };
        }

        public abstract string Name { get; }

        public Dictionary<string, string> Dictionary { get; private set; }

        public abstract string NewestProperties { get; }
        public abstract string CreateAccount { get; }
        public abstract string ForgotPassword { get; }
        public abstract string YouHaveNotAddedProperties { get; }
        public abstract string YouHaveNotUnactiveProperties { get; }
        public abstract string ExpiredProperties { get; }
        public abstract string Username { get; }
        public abstract string Password { get; }
        public abstract string ChooseImage { get; }
        public abstract string PriceInEUR { get; }
        public abstract string Sale { get; }
        public abstract string Rent { get; }
        public abstract string TotalBedRooms { get; }
        public abstract string TotalBathRooms { get; }
        public abstract string TotalGarages { get; }
        public abstract string TotalPropertiesCount { get; }
        public abstract string ExpirationDays { get; }
        public abstract string ContactModel { get; }
        public abstract string PropertyType { get; }

        public abstract string MostExpensiveProperties { get; }

        public abstract string AllProperties { get; }

        public abstract string HomePage { get; }

        public abstract string About { get; }

        public abstract string Properties { get; }

        public abstract string Blog { get; }

        public abstract string Pages { get; }

        public abstract string Contact { get; }

        public abstract string Register { get; }

        public abstract string Login { get; }

        public abstract string MyActiveProperties { get; }

        public abstract string IamAAgent { get; }

        public abstract string Admin { get; }

        public abstract string Profile { get; }

        public abstract string Add { get; }

        public abstract string Search { get; }

        public abstract string Title { get; }

        public abstract string Type { get; }

        public abstract string Location { get; }
        public abstract string SelectLocation { get; }

        public abstract string PopulatedPlace { get; }
        public abstract string SelectPopulatedPlace { get; }

        public abstract string DetailInfo { get; }

        public abstract string Size { get; }

        public abstract string Year { get; }

        public abstract string Floor { get; }

        public abstract string TotalFloors { get; }

        public abstract string BedRooms { get; }

        public abstract string BathRooms { get; }

        public abstract string Garages { get; }

        public abstract string BuildingType { get; }

        public abstract string Condition { get; }

        public abstract string Equipments { get; }

        public abstract string Details { get; }

        public abstract string Description { get; }

        public abstract string Options { get; }

        public abstract string Price { get; }

        public abstract string Expiration { get; }

        public abstract string IncreaseExpiration { get; }

        public abstract string MoreInfo { get; }

        public abstract string Contacts { get; }

        public abstract string Heating { get; }

        public abstract string Summary { get; }

        public abstract string Id { get; }

        public abstract string Amenities { get; }

        public abstract string ContactAgent { get; }
    }
}
