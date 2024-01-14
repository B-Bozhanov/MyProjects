namespace RealEstate.Services.TranslateService.Models
{
    public class Bulgarian : LanguageBase
    {
        public override string Name => nameof(Bulgarian);

        public override string NewestProperties { get => "Последно добавени"; }

        public override string Password { get => "Парола"; }

        public override string ForgotPassword { get => "Забравена парола"; }

        public override string AllProperties { get => "Всички обяви"; }

        public override string HomePage { get => "Начало"; }
        public override string CreateAccount { get => "Създай нов акаунт"; }

        public override string YouHaveNotAddedProperties { get => "Нямате активни обяви!"; }

        public override string YouHaveNotUnactiveProperties { get => "Нямате неактивни обяви!"; }

        public override string ExpiredProperties { get => "Изтекли Обяви"; }

        public override string About { get => "За нас"; }

        public override string Properties { get => "Обяви"; }

        public override string Blog { get => "Блог"; }

        public override string Pages { get => "Страници"; }

        public override string Contact { get => "Контакти"; }

        public override string Register { get => "Регистрация"; }

        public override string Login { get => "Вход"; }

        public override string MyActiveProperties { get => "Моите Активни Обяви"; }

        public override string IamAAgent { get => "Аз съм Агенция"; }

        public override string Admin { get => "Администрация"; }

        public override string Profile { get => "Моят Профил"; }

        public override string Add { get => "Добави"; }

        public override string Search { get => "Търсене"; }

        public override string Title { get => "Добави Обява"; }

        public override string Type { get => "Тип"; }

        public override string Location { get => "Град / Област"; }

        public override string Username { get => "Потребителско име"; }

        public override string PopulatedPlace  => "Населено място"; 

        public override string DetailInfo { get => "Детайлна информация"; }

        public override string Size { get => "Квадратура"; }

        public override string Year { get => "Година"; }

        public override string Floor { get => "Етаж"; }

        public override string TotalFloors { get => "Общо етажи"; }

        public override string TotalPropertiesCount { get => "Брой Обяви"; }

        public override string BedRooms { get => "Спални"; }

        public override string BathRooms { get => "Бани"; }

        public override string Garages { get => "Гаражи"; }

        public override string BuildingType { get => "Тип на строителство"; }

        public override string Condition { get => "Състояние"; }

        public override string Equipments { get => "Обзавеждане"; }

        public override string Details { get => "Детайли"; }

        public override string Description { get => "Описание"; }

        public override string Options { get => "Статус"; }

        public override string Price { get => "Цена в Евро"; }

        public override string Expiration { get => "Валидност"; }

        public override string IncreaseExpiration { get => "Увеличаване на валидност"; }

        public override string Contacts { get => "Контакти"; }

        public override string Heating { get => "Отопление"; }

        public override string Summary { get => "Описание"; }

        public override string Id { get => "Обява номер"; }

        public override string Amenities { get => "Удобства"; }

        public override string ContactAgent { get => "Лице за Контакт"; }

        public override string MostExpensiveProperties { get => "Най-Скъпи"; }

        public override string PropertyType => "Тип на обявата";

        public override string ExpirationDays => "Валидност в дни";

        public override string ContactModel => "Лице за контакт";

        public override string SelectLocation => "Изберете район";

        public override string SelectPopulatedPlace => "Изберете населено място";

        public override string TotalBedRooms => "Брой спални";

        public override string TotalBathRooms => "Брой бани";

        public override string TotalGarages => "Брой гаражи";

        public override string MoreInfo => "Повече информация";

        public override string Sale => "Продажба";

        public override string Rent => "Под наем";

        public override string PriceInEUR => "Цена в EUR";

        public override string ChooseImage => "Изберете снимка/и";
    }
}
