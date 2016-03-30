using Lemon.Core;
using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using System.Collections.Generic;

namespace eas.modules
{
    public class PowerBIHtmlNormalization : INormalize
    {
        public BsonDocument Normalize(Stream stream, IDictionary<string, object> context)
        {
            var metadata = new BsonDocument();

            var document = new HtmlAgilityPack.HtmlDocument();

            document.Load(stream, Encoding.GetEncoding("utf-8"));

            var scopeNode = document.DocumentNode.SelectSingleNode("//*[@id='link_0']");

            var message = document.DocumentNode.SelectSingleNode("//*[@class='first-message']");

            var dateNode = message.SelectSingleNode("//span[contains(@class,'lia-message-posted-on')]/span[@title]");

            string createdOnText = string.Empty;

            if (dateNode == null)
            {
                var tempNodes = message.SelectSingleNode("//span[contains(@class,'ia-message-posted-on')]").SelectNodes("span[contains(@class,'local')]");

                createdOnText = tempNodes[0].InnerText + " " + tempNodes[1].InnerText;
            }
            else
            {
                createdOnText = dateNode.Attributes["title"].Value;
            }

            var statics = message.SelectSingleNode("//div[contains(@class, 'lia-message-statistics')]").InnerHtml;

            var viewsText = new Regex(@"(\d+) Views").Match(statics).Groups[1].Value;

            var msgCountText = new Regex(@"of (\d+)").Match(statics).Groups[1].Value;

            var subject = message.SelectSingleNode("//div[@class='lia-message-subject']");

            var resolvedIcon = subject.SelectSingleNode("span[@class='solved']");

            var idDiv = message.SelectSingleNode("//*[@data-message-id]");

            var regex = new Regex(@"(\d+)-(\d+)-(\d{4})(.+)", RegexOptions.Multiline);

            var match = regex.Match(createdOnText);

            var formated = string.Format("{0}-{1}-{2}{3}", match.Groups[3].Value, match.Groups[1].Value, match.Groups[2].Value, match.Groups[4].Value);

            var forum = scopeNode.InnerText;

            var id = idDiv.Attributes["data-message-id"].Value;

            bool resovled = resolvedIcon != null;

            var createdOn = DateTime.Parse(formated);

            var title = subject.SelectSingleNode("h1").InnerText;

            var views = int.Parse(viewsText);

            var replies = int.Parse(msgCountText) - 1;

            metadata.Add("_id", string.Format("powerbi_{0}", id));

            metadata.Add("title", title);

            metadata.Add("forumId", string.Format("powerbi.{0}", forum.ToLower()));

            metadata.Add("answered", resovled);

            metadata.Add("createdOn", createdOn);

            metadata.Add("views", views);

            metadata.Add("replies", replies);

            return metadata;
        }
    }
}
