using Lemon.Core.Discover;
using System.Collections.Generic;
using MongoDB.Bson;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace eas.modules
{
    public class MSDNIndexBuilder : IBuildIndex
    {
        public IEnumerable<BsonDocument> Build(string text)
        {
            var htmlDocument = new HtmlDocument();

            var documents = new List<BsonDocument>();

            htmlDocument.LoadHtml(text);

            var nodes = htmlDocument.DocumentNode.SelectNodes("//*[@id=\"threadList\"]/li//div[@class='detailscontainer']");

            var numberRegex = new Regex(@"\d+");

            foreach(var node in nodes)
            {
                var h3link = node.SelectSingleNode("h3/a");

                var title = h3link.InnerText;

                var uri = h3link.Attributes["href"].Value + "&outputAs=xml";

                var id = h3link.Attributes["data-threadid"].Value;

                var metricsNode = node.SelectSingleNode("div[contains(@class, 'metrics')]");

                var state = metricsNode.SelectSingleNode("span[@class='statefilter']/a").InnerText;

                var replies = int.Parse(numberRegex.Match(metricsNode.SelectSingleNode("span[@class='replycount']").InnerText).Groups[0].Value);

                var views = int.Parse(numberRegex.Match(metricsNode.SelectSingleNode("span[@class='viewcount']").InnerText).Groups[0].Value);

                var document = new BsonDocument();

                document.Add("_id", id)
                        .Add("title", title)
                        .Add("uri", uri)
                        .Add("state", state)
                        .Add("replies", replies)
                        .Add("views", views);

                documents.Add(document);
            }

            return documents;
        }
    }
}
