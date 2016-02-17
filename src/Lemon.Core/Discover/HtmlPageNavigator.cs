using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lemon.Core.Discover
{
    internal class HtmlPageNavigator
    {
        private const int MAX_TRY_TIMES = 10;

        private readonly string _uriFormat;

        private HttpClient _httpClient;

        private int _currentIndex = 0;

        public HtmlPageNavigator(string uriFormat)
        {
            _uriFormat = uriFormat;

            _httpClient = new HttpClient();

            var headers = _httpClient.DefaultRequestHeaders;

            headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            headers.Add("Cache-Control", "keep-alive");
            headers.Add("Accept-Language", "en-US,en;q=0.8,zh-Hans-CN;q=0.5,zh-Hans;q=0.3");
            headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; rv:16.0) Gecko/20100101 Firefox/16.0");
        }

        public Task<Stream> GetAsync()
        {
            if (_currentIndex == 0)
            {
                return null;
            }

            var uriString = string.Format(_uriFormat, _currentIndex);

            var uri = new Uri(uriString);

            return RetryGetStreamAsync(uri);
        }

        private Task<Stream> RetryGetStreamAsync(Uri uri)
        {
            Task<Stream> result = null;

            int tryTimes = 0;

            while(result == null && tryTimes <= MAX_TRY_TIMES)
            {
                try
                {
                    result = _httpClient.GetStreamAsync(uri);
                }
                catch (Exception ex)
                {
                    tryTimes++;

                    System.Threading.Thread.Sleep(1000);
                }
            }

            if(tryTimes > MAX_TRY_TIMES)
            {
                throw new Exception("out of max try times");
            }

            return result;
        }
    

        public void NavigateTo(int pageIndex)
        {
            _currentIndex = pageIndex;
        }

        public void Next()
        {
            _currentIndex++;
        }

        public void Previous()
        {
            if (_currentIndex > 1)
            {
                _currentIndex--;
            }
        }
    }
}
