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

        var locationsNames = document.QuerySelectorAll(".form-wrap .form-input > option[value]");

        var locationsIdes = locationsNames
            .Select(x => x.OuterHtml.ToString()
                .Split(' ')[1]
                .Replace("value=\"", string.Empty)
                .Replace("\"", string.Empty))
            .Skip(1)
            .ToList();

        locationsIdes.Remove(locationsIdes.Last());

        var client = new RestClient("https://imoti.bg/bg/ajax/getLocations/");
        var request = new RestRequest();
        request.AddParameter("Get", "Post");

        var locations = new Dictionary<string, string>();

        foreach (var id in locationsIdes)
        {
            request.AddParameter("id", $"{id}");
            var response = client.Post(request).Content.Split("\r\n");

            foreach (var item in response)
            {
                await Console.Out.WriteLineAsync(item);
                break;
            }
            break;
        }


        //var content = response.Content; // Raw content as string
        //var response2 = client.Post<Person>(request);
        //var name = response2.Data.Name;
        //await Console.Out.WriteLineAsync(content);
    }
}