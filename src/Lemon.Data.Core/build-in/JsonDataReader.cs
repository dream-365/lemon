using System;
using System.IO;
using Newtonsoft.Json;

namespace Lemon.Transform
{
    public class JsonDataReader<TRecord> : IDataReader<TRecord>
    {
        private string _filePath;

        private bool _isOpen;

        private bool _end;

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

        public bool End()
        {
            return _end;
        }

        public TRecord Read()
        {
            if(!_isOpen)
            {
                Open();
            }

            try
            {
                var text = _reader.ReadLine();

                _end = _reader.EndOfStream;

                return JsonConvert.DeserializeObject<TRecord>(text, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
            }
            catch (Exception ex)
            {
                LogService.Default.Error("Json Data Input failed", ex);

                throw ex;
            }
        }

        public object ReadObject()
        {
            return Read();
        }

        public void Close()
        {
            if(_isOpen)
            {
                _reader.Close();
            }
        }
    }
}
