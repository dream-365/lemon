using MongoDB.Bson;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Lemon.Transform;

namespace Lemon.Core.Discover
{
    public class LithiumWebApiObject : ITransformDataReader
    {
        private Func<string> _acquireToken;

        private Action<BsonDocument> _forEach;

        private int _offset = 0;

        private int _limit = 10;

        private int _max = 100;

        public string DataPath { get; set; }

        public string PrimaryKey
        {
            get
            {
                return "id";
            }
        }

        private string _clientId;

        private string _tanentId;

        private string _baseUri;

        public LithiumWebApiObject(Func<string> acquireToken)
        {
            _acquireToken = acquireToken;

            _tanentId = "{your_tanentId}";

            _clientId = "{your_clientId}";

            _baseUri = "https://api.lithium.com/community/2.0";
        }

        private void Query(string requestUrl)
        {
            var httpClient = new HttpClient();

            var headers = httpClient.DefaultRequestHeaders;

            headers.Add("access_token", _acquireToken());

            headers.Add("client-id", _clientId);

            var response = httpClient.GetAsync(requestUrl).Result;

            var json = response.Content.ReadAsStringAsync().Result;

            var jsonObject = JsonConvert.DeserializeObject(json) as JObject;

            JToken value = jsonObject.SelectToken("data.items");

            var array = value as JArray;

            foreach(var item in array)
            {
                var text = item.ToString();

                var document = BsonDocument.Parse(text);

                _forEach(document);
            }
        }

        private string GetNextUrl()
        {
            var query = string.Format("SELECT * FROM messages LIMIT {0} OFFSET {1}", _limit, _offset);

            _offset = _offset + _limit;

            if(_offset >= _max)
            {
                return string.Empty;
            }

            return string.Format("{0}/{1}/search?q={2}", _baseUri, _tanentId, query);
        }

        public Task ForEachAsync(Action<BsonDocument> forEach)
        {
            _forEach = forEach;

            var task = Task.Run(() => {
                while (true)
                {
                    var url = GetNextUrl();

                    if(string.IsNullOrWhiteSpace(url))
                    {
                        break;
                    }

                    Query(url);
                }
            });

            return task;
        }

        public void ForEach(Action<BsonDataRow> forEach)
        {
            ForEachAsync((document) => {
                forEach(new BsonDataRow(document));
            }).Wait();
        }
    }
}
