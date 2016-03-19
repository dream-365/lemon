using Lemon.Core.Discover;
using System.Collections.Generic;
using MongoDB.Bson;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace eas.modules
{
    public class PowerBIIndexBuilder : IBuildIndex
    {
        public IEnumerable<BsonDocument> Build(string text)
        {
            var idPattern = new Regex(@"td-p/(\d+)/jump-to");

            var htmlDocument = new HtmlDocument();

            var documents = new List<BsonDocument>();

            htmlDocument.LoadHtml(text);

            var nodes = htmlDocument.DocumentNode.SelectNodes(@"//*[@id='messageList']/div/table/tbody/tr");

            foreach (var node in nodes)
            {
                var subjectNode = node.SelectSingleNode("td[contains(@class,'threadSubjectColumn')]//h2//a");

                var title = subjectNode.InnerText.Replace("\r\n", "").Trim();

                var uri = string.Format("http://community.powerbi.com{0}", subjectNode.Attributes["href"].Value);

                var id = idPattern.Match(uri).Groups[1].Value;


                var document = new BsonDocument();

                document.Add("_id", id)
                        .Add("title", title)
                        .Add("uri", uri);

                documents.Add(document);
            }

            return documents;
        }
    }
}
