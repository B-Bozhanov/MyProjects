namespace StartUp
{
    using AngleSharp;
    using AngleSharp.Dom;
    using Newtonsoft.Json;

    using StartUp.Constants;

    public class RegionScraperService
    {
        private string regionsUrl;
        private string downTownsUrl;
        private readonly IBrowsingContext context;
        private IDocument downTownsDocument = null!;
        private IDocument regionsDocument = null!;

        public RegionScraperService(string regionsUrl, string downTownsUrl, IBrowsingContext context)
        {
            if (string.IsNullOrWhiteSpace(regionsUrl) || string.IsNullOrWhiteSpace(downTownsUrl) || context == null)
            {
                throw new InvalidOperationException("The urls or context can not be empty");
            }

            this.regionsUrl = regionsUrl;
            this.downTownsUrl = downTownsUrl;
            this.context = context;
        }

        public async Task<IEnumerable<Region>> GetAreasAsync()
        {
            var regions = new List<Region>();

            this.downTownsDocument = await context.OpenAsync(this.downTownsUrl);
            this.regionsDocument = await context.OpenAsync(this.regionsUrl);

            var townsUrlExtencions = GetTownsExtencions(this.downTownsDocument, QuerySelectors.TownSelectors);
            var areasUrlExtencions = GetAreasExtencions(this.regionsDocument, QuerySelectors.AreaExtencions);

            var townsDistricts = this.GetTownsDistricts(townsUrlExtencions);

            Parallel.ForEach(areasUrlExtencions, areasUrlExtencion =>
             {
                 this.regionsUrl = $"https://www.ekatte.com/{areasUrlExtencion}";

                 var populatedPlaces = GetPopulatedPlacesAsync(this.context, this.regionsUrl, QuerySelectors.PopulatedPlaceName, QuerySelectors.TitleHtmlAttribute).Result;

                 var region = new Region().Parse(populatedPlaces, townsDistricts);

                 regions.Add(region);
             });

            return regions;
        }

        public void SaveAsJson(string path, IEnumerable<Region> regions)
        {
            var json = JsonConvert.SerializeObject(regions, Formatting.Indented);

            File.WriteAllText(@$"{path}.json", json);
        }

        private Dictionary<string, List<string>> GetTownsDistricts(List<string> townsUrlExtencions)
        {
            var townsDistricts = new Dictionary<string, List<string>>();

            Parallel.ForEach(townsUrlExtencions, townUrlExtencion =>
            {
                var townName = $"гр.{townUrlExtencion.Split('_').Last()}";
                this.downTownsUrl = $"https://bg.wikipedia.org/wiki/%D0%9A%D0%B0%D1%82%D0%B5%D0%B3%D0%BE%D1%80%D0%B8%D1%8F:{townUrlExtencion}";

                var currentTownDistricts = GetDistrictsAsync(this.context, this.downTownsUrl).Result;

                townsDistricts.Add(townName, currentTownDistricts);
            });

            return townsDistricts;
        }

        private static List<string> GetTownsExtencions(IDocument document, string townSelectors)
        {
            return document
                .QuerySelectorAll(townSelectors)
                .Select(x => x.TextContent
                        .Replace(' ', '_'))
                .ToArray()
                .Skip(1)
                .ToList();
        }

        private static List<string> GetAreasExtencions(IDocument document, string regionLinkSelectors)
        {
            return document
                 .QuerySelectorAll(regionLinkSelectors)
                 .Select(x => ($"{"област-"}{x.TextContent
                    .Replace(' ', '-').TrimEnd()}")
                    .Replace("(", string.Empty)
                    .Replace(")", string.Empty))
                 .ToList();
        }

        private static async Task<List<string>> GetDistrictsAsync(IBrowsingContext context, string townUrl)
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

        private static async Task<List<string>> GetPopulatedPlacesAsync(IBrowsingContext context, string url, string placeNamesSelectors, string htmlAttribute)
        {
            var document = await context.OpenAsync(url);
            var PopulatedPlaces = document
                .QuerySelectorAll(placeNamesSelectors)
                .Select(x => x.GetAttribute(htmlAttribute))
                .ToList();

            return PopulatedPlaces!;
        }
    }
}
