namespace RealEstate.Services.PropertyScraper
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AngleSharp;

    using RealEstate.Services.Interfaces;

    public class PropertyScraper : IPropertyScraper
    {
        public async Task<string> GetAsJson(string url)
        {
            url = "https://imoti.bg/%D0%BF%D1%80%D0%BE%D0%B4%D0%B0%D0%B6%D0%B1%D0%B8";

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            var document = await context.OpenAsync(url);

            var pages = document.QuerySelectorAll(".col-sm-12 > ul.pagination-custom select option")
                .Select(x => x.TextContent)
                .ToArray();

            foreach (var page in pages)
            {
                document = await context.OpenAsync($"{url}/page:{page}");
                var propertiesLinks = document.QuerySelectorAll("h4.product-classic-title a[href]").Select(x => x.GetAttribute("href"));

                foreach (var propertyLink in propertiesLinks)
                {
                    var propertyDoc = await context.OpenAsync(propertyLink);

                    var propertyType = propertyDoc.QuerySelector("h4.ttitle");

                    var propertyInfo = propertyDoc.QuerySelectorAll("table.details_table .list-terms-inline > td")
                        .Select(x => x.TextContent.Trim()).ToList();

                    var region = string.Empty;
                    var populatedPlace = string.Empty;
                    var price = string.Empty;
                    var pricePerSquareM = string.Empty;
                    var size = string.Empty;
                    var floor = string.Empty;
                    var visitors = string.Empty;


                    for (int i = 1; i < propertyInfo.Count; i += 2)
                    {

                    }
                    Console.WriteLine("==============================================");

                }
            }
                    return null;
        }
    }
}
