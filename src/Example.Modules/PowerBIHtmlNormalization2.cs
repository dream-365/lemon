using Lemon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace eas.modules
{
    public class PowerBIHtmlNormalization2 : INormalize
    {
        public BsonDocument Normalize(Stream stream)
        {
            var metadata = new BsonDocument();

            var document = new HtmlDocument();

            document.Load(stream, Encoding.GetEncoding("utf-8"));

            var scopeNode = document.DocumentNode.SelectSingleNode("//*[@id='link_0']");

            ThrowExceptionIfNull(scopeNode, "can not find the socpe node");

            var messageListNode = document.DocumentNode.SelectSingleNode("//div[contains(@class,'linear-message-list')]");

            ThrowExceptionIfNull(messageListNode, "can not find the message list node");

            var messageViewNodes = messageListNode.SelectNodes("//div[contains(@class,'lia-linear-display-message-view')]");

            ThrowExceptionIfNull(messageViewNodes, "can not find the message view nodes");

            var messages = new List<BsonDocument>();

            foreach(var messageViewNode in messageViewNodes)
            {
                BsonDocument message = ParseMessageViewNode(messageViewNode);

                messages.Add(message);
            }

            var firstMessage = messages.FirstOrDefault();

            var resovled = messages.Any(m => m.GetValue("isAnswer").AsBoolean);

            var forum = scopeNode.InnerText;

            var thread = new BsonDocument {
                { "_id", string.Format("powerbi_{0}", firstMessage.GetValue("id").AsString)},
                { "title", firstMessage.GetValue("title")},
                { "answered", resovled},
                { "author", firstMessage.GetValue("author")},
                { "createdOn", firstMessage.GetValue("createdOn")},
                { "body", firstMessage.GetValue("body")},
                { "messages", new BsonArray(messages.Skip(1))},
                { "forumId", string.Format("powerbi.{0}", forum.ToLower())}
            };

            return thread;
        }

        private BsonDocument ParseMessageViewNode(HtmlNode messageViewNode)
        {
            var mainContentNode = messageViewNode.SelectSingleNode("div//div[contains(@class,'lia-quilt-column-main-right')]");

            var messageAuthorNode = messageViewNode.SelectSingleNode("div//div[contains(@class, 'lia-message-author')]");

            var idNode = messageViewNode.SelectSingleNode("div//div[@data-message-id]");

            ThrowExceptionIfNull(idNode, "can not find the id node");

            ThrowExceptionIfNull(mainContentNode, "can not find the content node");

            var subjectNode = mainContentNode.SelectSingleNode("div//div[contains(@class, 'lia-message-subject')]");

            ThrowExceptionIfNull(subjectNode, "can not find the subject node");

            var titleNode = subjectNode.SelectSingleNode("h1");

            var solutionNode = subjectNode.SelectSingleNode("span[contains(@class, 'solution')]");

            var messagePostDateNode = mainContentNode.SelectSingleNode("div//span[contains(@class,'lia-message-posted-on')]");

            var bodyNode = mainContentNode.SelectSingleNode("div//div[contains(@class, 'lia-message-body-content')]");

            var userNameNode = messageAuthorNode.SelectSingleNode("div//a[contains(@class, 'lia-user-name-link')]/span");

            var userRegisterDateNode = messageAuthorNode.SelectSingleNode("div//span[contains(@class, 'DateTime')]");

            var author = new BsonDocument
            {
                { "name", userNameNode.InnerText},
                { "registeredOn", ParseDateTimeNode(userRegisterDateNode)}
            };

            var message = new BsonDocument
                {
                    { "id", idNode.Attributes["data-message-id"].Value },
                    { "body", bodyNode == null ? string.Empty : bodyNode.InnerHtml},
                    { "isAnswer", solutionNode != null },
                    { "createdOn", ParseDateTimeNode(messagePostDateNode)},
                    { "author", author }
                };

            if(titleNode != null)
            {
                message.Add("title", titleNode.InnerText);
            }

            return message;
        }


        private DateTime ParseDateTimeNode(HtmlNode dateTimeNode)
        {
            var node = dateTimeNode.SelectSingleNode("span[@class='local-friendly-date']");

            var text = string.Empty;

            if (node == null)
            {
                node = dateTimeNode.SelectSingleNode("span[@class='local-date']");

                text = node.InnerText;
            }
            else
            {
                text = node.Attributes["title"].Value;
            }

            var dateTimeFormatPattern = new Regex(@"(\d+)-(\d+)-(\d{4})(.+)", RegexOptions.Multiline);

            var dateFormatPattern = new Regex(@"(\d+)-(\d+)-(\d{4})");

            var dateTimeMatch = dateTimeFormatPattern.Match(text);

            var formatedDateText = string.Empty;

            if (dateTimeMatch.Success)
            {
                formatedDateText = string.Format("{0}-{1}-{2}{3}", dateTimeMatch.Groups[3].Value, dateTimeMatch.Groups[1].Value, dateTimeMatch.Groups[2].Value, dateTimeMatch.Groups[4].Value);
            }
            else
            {
                var dateMatch = dateFormatPattern.Match(text);

                if(!dateMatch.Success)
                {
                    throw new Lemon.Core.Exception.NormalizeException(string.Format("invalid date format {0}", text));
                }

                formatedDateText = string.Format("{0}-{1}-{2}", dateMatch.Groups[3].Value, dateMatch.Groups[1].Value, dateMatch.Groups[2].Value);
            }

            return DateTime.Parse(formatedDateText);
        }

        protected void ThrowExceptionIfNull(object obj, string message)
        {
            if(obj == null)
            {
                throw new Lemon.Core.Exception.NormalizeException(message);
            }
        }
    }
}
