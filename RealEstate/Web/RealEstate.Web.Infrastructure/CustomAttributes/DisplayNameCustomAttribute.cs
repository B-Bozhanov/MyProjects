namespace RealEstate.Web.Infrastructure.CustomAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RealEstate.Services.Interfaces;
    using static RealEstate.Common.GlobalConstants;

    public class DisplayNameCustomAttribute : DisplayNameAttribute
    {
        private readonly ITranslateService translateService;

        public DisplayNameCustomAttribute(ITranslateService translateService)
        {
            this.translateService = translateService;
        }
        public DisplayNameCustomAttribute(string displayName)
        {
            this.DisplayNameValue = displayName;
        }

        public override string DisplayName => this.GetTranslatedDisplayName(this.DisplayNameValue);

        private string GetTranslatedDisplayName(string displayNameValue)
        {
            var translatedWord = this.translateService.TranslateTo(CurrentSystemLanguage, displayNameValue);
            return translatedWord;
        }
    }
}
