using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lemon.Transform
{
    public class JsonFileDataReader : ITransformDataReader
    {
        private string _primaryKey;

        private string _filePath;

        public string PrimaryKey
        {
            get
            {
                return _primaryKey;
            }
        }

        public JsonFileDataReader(string filePath, string primaryKey)
        {
            _filePath = filePath;

            _primaryKey = primaryKey;
        }

        public void ForEach(Action<IValueProvider> forEach)
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

                        forEach(new BsonDocumentValueProvider(document));
                    }
                    catch (Exception ex)
                    {
                        var bak = Console.ForegroundColor;

                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine();

                        Console.WriteLine(ex.Message);

                        Console.ForegroundColor = bak;
                    }
                }
            }
        }
    }
}
