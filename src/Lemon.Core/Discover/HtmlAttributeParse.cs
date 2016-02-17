using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace Lemon.Core.Discover
{
    public class HtmlAttributeParse
    {
        private readonly string _xpath;

        private readonly string _attribute;

        public HtmlAttributeParse(string xpath, string attribute)
        {
            _xpath = xpath;

            _attribute = attribute;
        }

        public IEnumerable<string> Parse(string content)
        {
            var result = new List<string>();

            var htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(content);

            var nodes = htmlDocument.DocumentNode.SelectNodes(_xpath);

            if (nodes == null)
            {
                Console.WriteLine("No nodes found in the current content");

                return result;
            }

            foreach (HtmlNode node in nodes)
            {
                var link = node.GetAttributeValue(_attribute, "");

                if (!string.IsNullOrEmpty(link))
                {
                    result.Add(link);
                }
            }

            return result;
        }
    }
}
