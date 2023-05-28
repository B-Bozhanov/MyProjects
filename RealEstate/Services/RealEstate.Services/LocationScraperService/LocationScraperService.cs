namespace RealEstate.Services.LocationScraperService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AngleSharp;

    using Newtonsoft.Json;

    using RealEstate.Services.Interfaces;
    using RealEstate.Services.RegionScraperService.Models;

    using RestSharp;

    public class LocationScraperService : IRegionScraperService
    {
        public async Task<string> GetAllAsJason()
        {
            var locations = await GetLocatios();

            var json = JsonConvert.SerializeObject(locations);

            return json;
        }

        public async Task<IEnumerable<Location>> GetRegionsAsync(string country = null)
        {
            return await GetLocatios();
        }

        private static async Task<IEnumerable<Location>> GetLocatios()
        {
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

                    location.PopulatedPlaces.Add(new PopulatedPlace(currentPlace));
                }

                locations.Add(location);
            }

            return locations;
        }
    }
}
