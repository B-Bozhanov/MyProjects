using System.Text;

using AngleSharp;
using AngleSharp.Common;
using AngleSharp.Io;
using AngleSharp.Text;

using RestSharp;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        // TODO: Take all with only one package!

        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);

        var document = await context.OpenAsync("https://imoti.bg/");

        var locationsElements = document.QuerySelectorAll(".form-wrap .form-input > option[value]");

        var locationsNames = locationsElements
            .Select(x => x.TextContent)
            .Skip(1)
            .SkipLast(1)
            .ToList();

        var locationsIdes = locationsElements
            .Select(x => x.OuterHtml.ToString()
                .Split(' ')[1]
                .Replace("value=\"", string.Empty)
                .Replace("\"", string.Empty))
            .Skip(1)
            .SkipLast(1)
            .ToList();

        var client = new RestClient("https://imoti.bg/bg/ajax/getLocations/");

        var request = new RestRequest();

        request.AddParameter("Get", "Post");

        var locations = new List<Location>();

        for (int id = 0; id < locationsIdes.Count; id++)
        {
            request.AddParameter("id", $"{locationsIdes[id]}");

            var response = client.Post(request).Content!
                .Split(new char[2] { '\n', '\t' })
                .ToList();

            var location = new Location
            {
                Name = locationsNames[id],
            };

            for (int j = 3; j < response.Count - 2; j += 2)
            {
                var currentPlace = response[j].Replace("</option>", string.Empty).Split('>', StringSplitOptions.RemoveEmptyEntries)[1];

                location.PopulatedPlaces.Add(currentPlace);
            }

            locations.Add(location);
        }
    }

    public class Location
    {
        public Location()
        {
            this.PopulatedPlaces = new List<string>();
        }

        public string Name { get; set; } = null!;

        public List<string> PopulatedPlaces { get; set; }
    }
}