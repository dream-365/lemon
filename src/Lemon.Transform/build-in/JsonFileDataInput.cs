using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public class JsonFileDataInput : AbstractDataInput
    {
        private string _filePath;

        public JsonFileDataInput(string filePath)
        {
            _filePath = filePath;
        }

        public async Task ForEach(Func<BsonDataRow, Task<bool>> forEach)
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

                        await forEach(new BsonDataRow(document));
                    }
                    catch (Exception ex)
                    {
                        LogService.Default.Error("Json Data Input failed", ex);
                    }
                }
            }
        }

        public override async Task StartAsync(IDictionary<string, object> parameters = null)
        {
            await ForEach(SendAsync);

            Complete();
        }
    }
}
