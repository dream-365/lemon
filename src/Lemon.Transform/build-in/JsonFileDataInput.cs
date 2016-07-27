using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lemon.Transform
{
    public class JsonFileDataInput : AbstractDataInput
    {
        private string _filePath;

        public JsonFileDataInput(string filePath)
        {
            _filePath = filePath;
        }

        public void ForEach(Action<BsonDataRow> forEach)
        {
            using (var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            using (var sr = new StreamReader(fs))
            {
                var documents = new List<BsonDocument>();

                while (!sr.EndOfStream)
                {
                    try
                    {
                        var text = sr.ReadLine();

                        var document = BsonDocument.Parse(text);

                        forEach(new BsonDataRow(document));
                    }
                    catch (Exception ex)
                    {
                        LogService.Default.Error("Json Data Input failed", ex);
                    }
                }
            }
        }

        public override void Start(IDictionary<string, object> parameters = null)
        {
            ForEach(Post);

            Complete();
        }
    }
}
