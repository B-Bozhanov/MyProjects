namespace RealEstate.Services.TranslateService.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using RealEstate.Services.Interfaces;

    public abstract class SupportLanguage
    {
        private readonly Dictionary<string, BaseLanguage> supportedLanuages = new Dictionary<string, BaseLanguage>
        {
            {nameof(Bulgarian), new Bulgarian() },
            {nameof(English), new English() },
        };

        public BaseLanguage GetSupportedLanguage(string languageName)
        {
            if (!this.supportedLanuages.ContainsKey(languageName))
            {
                return null;
            }

            return this.supportedLanuages[languageName];
        }
    }
}
