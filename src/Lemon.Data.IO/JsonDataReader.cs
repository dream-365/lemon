using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using Lemon.Data.Core;

namespace Lemon.Data.IO
{
    public class JsonDataReader<TRecord> : IDataReader<TRecord>
    {
        private string _filePath;

        private bool _isOpen;
        
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
            throw new NotImplementedException();
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

                return JsonConvert.DeserializeObject<TRecord>(text, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
            }
            catch (Exception ex)
            {
                LogService.Default.Error("Json Data Input failed", ex);

                throw ex;
            }
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

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }
    }
}
