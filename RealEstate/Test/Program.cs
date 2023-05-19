using System.Text;

using AngleSharp;

using Newtonsoft.Json;

namespace Test
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var url = "https://imoti.bg/%D0%BF%D1%80%D0%BE%D0%B4%D0%B0%D0%B6%D0%B1%D0%B8";

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

                    var region = propertyInfo[1];
                    var populatedPlace = propertyInfo[3];
                    var priceStr = propertyInfo[5];
                    var pricePerSquareM = propertyInfo[7];
                    var size = propertyInfo[9];
                    string floor = null!;
                    string maxFloors = null!;

                    if (propertyInfo.Contains("Етаж"))
                    {
                        var floosInfo = propertyInfo[11].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        floor = floosInfo[0];
                        if (floosInfo.Length > 1)
                        {
                        maxFloors = floosInfo[2];
                        }
                    }

                    var price = priceStr.Replace(" ", string.Empty);
                    await Console.Out.WriteLineAsync(region);
                    await Console.Out.WriteLineAsync(populatedPlace);
                    Console.WriteLine(priceStr);
                    Console.WriteLine(pricePerSquareM);
                    Console.WriteLine(size);
                    await Console.Out.WriteLineAsync(floor);
                    await Console.Out.WriteLineAsync(maxFloors);
                    //Thread.Sleep(1000);
                    Console.WriteLine(new string('=', 40));

                    // var property = new Property(region, populatedPlace, price, pricePerSquareM, size, floor);

                }
            }
        }

        private class Property
        {
            internal Property(string region, string populatedPlace, decimal price, decimal pricePerSquareM, int? size, string floor)
            {
                this.Region = region;
                this.PopulatedPlace = populatedPlace;
                this.Price = price;
                this.PricePerSquareM = pricePerSquareM;
                this.Size = size;
                this.Floor = floor;
            }

            public string Region { get; }
            public string PopulatedPlace { get; }
            public decimal Price { get; }
            public decimal PricePerSquareM { get; }
            public int? Size { get; }
            public string Floor { get; }
        }
    }
}