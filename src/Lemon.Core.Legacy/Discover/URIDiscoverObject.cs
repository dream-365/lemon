using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Lemon.Core.Discover
{
    public class URIDiscoverObject
    {
        private URINavigator _navigator;

        private int _start;

        private int _length;

        private Encoding _encoding;

        public IFindUri Finder { get; set; }

        private HttpClient _httpClient;

        public URIDiscoverObject(DiscoverSetting setting)
        {
            _navigator = new URINavigator(setting.UriTemplate, setting.Step);

            _encoding = Encoding.GetEncoding(setting.Encoding);

            _start = setting.Start;

            _length = setting.Length;

            _httpClient = new HttpClient();

            var headers = _httpClient.DefaultRequestHeaders;

            headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            headers.Add("Cache-Control", "keep-alive");
            headers.Add("Accept-Language", "en-US,en;q=0.8,zh-Hans-CN;q=0.5,zh-Hans;q=0.3");
            headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; rv:16.0) Gecko/20100101 Firefox/16.0");
        }
        public async Task ForEachAsync(Action<string> forEach)
        {
            for(int i = 0; i < _length; i++)
            {
                var uri = _navigator.Goto(_start + i);

                using (var stream = await _httpClient.GetStreamAsync(uri))
                using (var sr = new StreamReader(stream, _encoding))
                {
                    var text = await sr.ReadToEndAsync();

                    Finder.Find(text).ToList().ForEach(forEach);
                }
            }
        }
    }
}
