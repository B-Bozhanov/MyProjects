namespace RealEstate.Services.Interfaces
{
    public interface ITranslateService
    {
        public string TranslateTo(string translateToLanguage, string wordToTranslate);
    }
}
