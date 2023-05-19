namespace RealEstate.Services.RegionScraperService
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using AngleSharp;
    using AngleSharp.Dom;

    using Newtonsoft.Json;

    using RealEstate.Services.Interfaces;
    using RealEstate.Services.RegionScraperService.Constants;
    using RealEstate.Services.RegionScraperService.Models;

    public class RegionScraperService : IRegionScraperService
    {
        private readonly IBrowsingContext context;
        private string regionsUrl;
        private string downTownsUrl;
        private IDocument downTownsDocument = null!;
        private IDocument regionsDocument = null!;

        public RegionScraperService()
        {
            // TODO: Get dependencies from out.
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            // "https://bg.wikipedia.org/wiki/%D0%9A%D0%B0%D1%82%D0%B5%D0%B3%D0%BE%D1%80%D0%B8%D1%8F:%D0%9A%D0%B2%D0%B0%D1%80%D1%82%D0%B0%D0%BB%D0%B8_%D0%B2_%D0%91%D1%8A%D0%BB%D0%B3%D0%B0%D1%80%D0%B8%D1%8F";

            // "https://www.ekatte.com/";

            this.regionsUrl = Urls.RegionsUrl;
            this.downTownsUrl = Urls.DownTownsUrl;

            this.context = context;
        }

        public async Task<IEnumerable<Region>> GetRegionsAsync(string country = null)
        {
            var regions = new List<Region>();

            this.downTownsDocument = await this.context.OpenAsync(this.downTownsUrl);
            this.regionsDocument = await this.context.OpenAsync(this.regionsUrl);

            var townsUrlExtencions = this.GetTownsExtencions(this.downTownsDocument, QuerySelectors.TownSelectors);
            var areasUrlExtencions = this.GetAreasExtencions(this.regionsDocument, QuerySelectors.AreaExtencions);

            var townsDistricts = this.GetTownsDistricts(townsUrlExtencions);

            Parallel.ForEach(areasUrlExtencions, areasUrlExtencion =>
            {
                this.regionsUrl = $"https://www.ekatte.com/{areasUrlExtencion}";

                var populatedPlaces = this.GetPopulatedPlacesAsync(this.context, this.regionsUrl, QuerySelectors.PopulatedPlaceName, QuerySelectors.TitleHtmlAttribute).Result;

                var region = new Region().Parse(populatedPlaces, townsDistricts);

                regions.Add(region);
            });

            return regions;
        }

        public async Task<string> GetAllAsJason()
        {
            var regions = await this.GetRegionsAsync();

            var json = JsonConvert.SerializeObject(regions, Formatting.Indented);

            return json;
        }

        private Dictionary<string, List<string>> GetTownsDistricts(List<string> townsUrlExtencions)
        {
            var townsDistricts = new Dictionary<string, List<string>>();

            Parallel.ForEach(townsUrlExtencions, townUrlExtencion =>
            {
                var townName = $"гр.{townUrlExtencion.Split('_').Last()}";
                this.downTownsUrl = $"https://bg.wikipedia.org/wiki/%D0%9A%D0%B0%D1%82%D0%B5%D0%B3%D0%BE%D1%80%D0%B8%D1%8F:{townUrlExtencion}";

                var currentTownDistricts = this.GetDistrictsAsync(this.context, this.downTownsUrl).Result;

                townsDistricts.Add(townName, currentTownDistricts);
            });

            return townsDistricts;
        }

        private List<string> GetTownsExtencions(IDocument document, string townSelectors)
            => document
                 .QuerySelectorAll(townSelectors)
                 .Select(x => x.TextContent
                         .Replace(' ', '_'))
                 .ToArray()
                 .Skip(1)
                 .ToList();

        private List<string> GetAreasExtencions(IDocument document, string regionLinkSelectors)
            => document
                 .QuerySelectorAll(regionLinkSelectors)
                 .Select(x => $"{"област-"}{x.TextContent
                    .Replace(' ', '-').TrimEnd()}"
                    .Replace("(", string.Empty)
                    .Replace(")", string.Empty))
                 .ToList();

        private async Task<List<string>> GetDistrictsAsync(IBrowsingContext context, string townUrl)
        {
            var document = await context.OpenAsync(townUrl);

            var districts = document
            .QuerySelectorAll(QuerySelectors.Districs)
            .Select(x => x.TextContent
                    .Split('(')[0]
                    .Trim())
            .ToList();

            return districts;
        }

        private async Task<List<string>> GetPopulatedPlacesAsync(IBrowsingContext context, string url, string placeNamesSelectors, string htmlAttribute)
        {
            var document = await context.OpenAsync(url);
            var populatedPlaces = document
                .QuerySelectorAll(placeNamesSelectors)
                .Select(x => x.GetAttribute(htmlAttribute))
                .ToList();

            return populatedPlaces!;
        }
    }
}
