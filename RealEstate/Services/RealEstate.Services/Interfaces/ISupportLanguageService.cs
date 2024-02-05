namespace RealEstate.Services.Interfaces
{
    using RealEstate.Services.TranslateService.Models;

    public interface ISupportLanguageService
    {
        public BaseLanguage GetSupportedLanguage(string languageName);
    }
}
