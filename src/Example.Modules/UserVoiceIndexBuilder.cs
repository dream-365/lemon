using Lemon.Core.Discover;
using System.Collections.Generic;
using MongoDB.Bson;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System;

namespace eas.modules
{
    public class UserVoiceIndexBuilder : IBuildIndex
    {
        public IEnumerable<BsonDocument> Build(string text)
        {
            var htmlDocument = new HtmlDocument();

            var documents = new List<BsonDocument>();

            htmlDocument.LoadHtml(text);

            var parentnodes = htmlDocument.DocumentNode.SelectNodes("//ol[contains(@class, 'uvList')]");

            if (parentnodes == null)
            {
                return documents;
            }

            foreach (HtmlNode parentnode in parentnodes)
            {
                var nodes = htmlDocument.DocumentNode.SelectNodes("//li[contains(@class, 'uvIdea')]");

                if (nodes == null)
                {
                    return documents;
                }

                var numberRegex = new Regex(@"\d+");

                foreach (var node in nodes)
                {
                    var linkelement = node.SelectSingleNode("div[contains(@class, 'uvIdeaHeader')]/h2/a");

                    var title = linkelement.InnerText;

                    var uri = "https://powerapps.uservoice.com" + linkelement.Attributes["href"].Value;

                    var vote = int.Parse(numberRegex.Match(node.SelectSingleNode("div[contains(@class, 'uvIdeaVoteBadge')]//div[contains(@class, 'uvIdeaVoteCount')]").InnerText).Groups[0].Value);

                    //var id = h3link.Attributes["data-threadid"].Value;

                    //var metricsNode = node.SelectSingleNode("div[contains(@class, 'metrics')]");

                    //var state = metricsNode.SelectSingleNode("span[@class='statefilter']/a").InnerText;

                    //var replies = int.Parse(numberRegex.Match(metricsNode.SelectSingleNode("span[@class='replycount']").InnerText).Groups[0].Value);

                    //var views = int.Parse(numberRegex.Match(metricsNode.SelectSingleNode("span[@class='viewcount']").InnerText).Groups[0].Value);

                    var document = new BsonDocument();

                    document.Add("title", title)
                            .Add("uri", uri)
                            .Add("vote", vote)
                            ;

                    documents.Add(document);
                }
            }
            
            return documents;
        }
    }
}
