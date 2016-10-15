using System;
using System.IO;
using Newtonsoft.Json;
using Lemon.Data.Core;

namespace Lemon.Data.IO
{
    public class JsonDataReader<TRecord> : IDataReader<TRecord>
    {
        private string _filePath;

        private bool _isOpen;

        private TRecord _current;

        private StreamReader _reader;

        public JsonDataReader(string filePath)
        {
            _filePath = filePath;
        }

        private void Open()
        {
            var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);

            _reader = new StreamReader(fs);

            _isOpen = true;
        }

        public bool Next()
        {
            if (!_isOpen)
            {
                Open();
            }

            try
            {
                if (_reader.EndOfStream)
                {
                    return false;
                }

                var text = _reader.ReadLine();

                _current = JsonConvert.DeserializeObject<TRecord>(text, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });

                return true;
            }
            catch (Exception ex)
            {
                LogService.Default.Error("Json Data Input failed", ex);

                throw ex;
            }
        }

        public TRecord Read()
        {
            return _current;
        }

        object IDataReader.Read()
        {
            return Read();
        }

        public void Dispose()
        {
            if (_isOpen)
            {
                _reader.Close();
            }
        }
    }
}
