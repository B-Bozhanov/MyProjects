using CsvHelper;
using HtmlAgilityPack;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using StartUp;
using PuppeteerSharp;
using AngleSharp;
using AngleSharp.Dom;

//var httpClient = new HttpClient();
//var response = await httpClient.GetAsync("https://en.wikipedia.org/wiki/Greece");
//var content1 = await response.Content.ReadAsStringAsync();

using var browserFetcher = new BrowserFetcher();
await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
var browser = await Puppeteer.LaunchAsync(new LaunchOptions
{
    Headless = true
});
var page = await browser.NewPageAsync();
await page.GoToAsync("https://en.wikipedia.org/wiki/Greece");

var content = await page.GetContentAsync();

var context = BrowsingContext.New(Configuration.Default);
var document = await context.OpenAsync(req => req.Content(content));

var info = document.QuerySelectorAll("*").Where(e => e.LocalName == "div" && e.ClassName == "mw-parser-output");

foreach (var item in info)
{
    var test = item.Text().Split("\n").Select(t => t.Trim()).Where(t => !string.IsNullOrWhiteSpace(t));
    foreach (var t in test)
    {
        Console.WriteLine(t);
    }
}
Console.WriteLine(content);
//HtmlWeb web = new HtmlWeb();
//HtmlDocument document = web.Load("https://en.wikipedia.org/wiki/Greece");

//var headerNames = document.DocumentNode.SelectNodes("//div[@class='vector-pinned-container']");

//var titles = new List<Row>();

//foreach (var item in headerNames)
//{
//    titles.Add(new Row { Title = item.InnerText });
//}

//using (var writer = new StreamWriter("test.csv"))
//using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
//{
//    csv.WriteRecords(titles);
//}

//Console.WriteLine();