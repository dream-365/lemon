using HtmlAgilityPack;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lemon.Core.Data
{
    public class HtmlTableCollection : IReadOnlyCollection<BsonDocument>
    {
        private string _source;

        private string _rowXpath;

        public HtmlTableCollection(string source, string rowXpath)
        {
            _source = source;

            _rowXpath = rowXpath;
        }

        public Task ForEachAsync(Action<BsonDocument> processor)
        {
            var task = new Task(() => {
                var html = File.ReadAllText(_source);

                HtmlDocument document = new HtmlDocument();

                document.LoadHtml(html);

                var nodes = document.DocumentNode.SelectNodes(_rowXpath);

                var firstRowNode = nodes.FirstOrDefault();

                var headers = ParseCells(firstRowNode);

                foreach (var row in nodes.Skip(1))
                {
                    var bson = new BsonDocument();

                    var values = ParseCells(row);

                    for (int i = 0; i < headers.Length; i++)
                    {
                        bson.Set(headers[i], values[i]);
                    }

                    processor(bson);
                }
            });

            task.RunSynchronously();

            return task;
        }

        private static string[] ParseCells(HtmlNode rowNode)
        {
            var cells = rowNode.SelectNodes("td");

            var values = new List<string>();

            foreach (var cell in cells)
            {
                values.Add(cell.InnerText);
            }

            return values.ToArray();
        }

        public Task ForEachAsync(Func<BsonDocument, Task> processor)
        {
            throw new NotImplementedException();
        }
    }
}
