namespace RealEstate.Services.TranslateService
{
    using System.Collections.Generic;

    using RealEstate.Services.Interfaces;
    using RealEstate.Services.TranslateService.Models;

    public class TranslateService : ITranslateService
    {
        private readonly Dictionary<string, BaseLanguage> supportedLanuages = new Dictionary<string, BaseLanguage>
        {
            {nameof(Bulgarian), new Bulgarian() },
            {nameof(English), new English() },
        };

        public string TranslateTo(string translateToLanguage, string wordToTranslate)
        {
            var language = this.GetSupportedLanguage(translateToLanguage);

            if (language == null)
            {
                return "Not supported language!";
            }

            return TranslateWord(language, wordToTranslate);
        }

        private static string TranslateWord(BaseLanguage language, string wordToTranslate)
        {
            if (language.Dictionary.ContainsKey(wordToTranslate))
            {
                return language.Dictionary[wordToTranslate];
            }
            else
            {
                return "The selected word is not in this dictionary"; //TODO: Return the base language:
            }
        }

        private BaseLanguage GetSupportedLanguage(string languageName)
        {
            if (!this.supportedLanuages.ContainsKey(languageName))
            {
                return null;
            }

            return this.supportedLanuages[languageName];
        }
    }
}
