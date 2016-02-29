using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lemon.Core.Discover
{
    public class PaginationDiscovery : IDiscover
    {
        protected readonly GeneralXPatUriFindSetting _config;

        protected Task _currentRunningTask;

        protected bool _isCanceled = false;

        public PaginationDiscovery(GeneralXPatUriFindSetting config)
        {
            _config = config;
        }

        public event Action<string> OnDiscovered;

        public void Start()
        {
            _currentRunningTask = Dsicover();

            _currentRunningTask.Wait();
        }

        protected async Task Dsicover()
        {
            var navigation = new HtmlPageNavigator(_config.UrlFormat);

            int start = _config.Start;

            int end = _config.Start + _config.Length;

            var encoding = Encoding.GetEncoding(_config.Encoding);

            for (int i = start; i < end; i++)
            {
                if(_isCanceled)
                {
                    return;
                }

                navigation.NavigateTo(i);

                try
                {
                    string text = string.Empty;

                    using (var content = await navigation.GetAsync())
                    using (var sr = new StreamReader(content, encoding))
                    {
                        text = await sr.ReadToEndAsync();
                    }

                    Parse(text);
                }
                catch (Exception ex)
                {
                    continue;
                }               
            }
        }

        private void Parse(string text)
        {
            var htmlAttributes = new HtmlAttributeParse(_config.LookUp.XPath, _config.LookUp.Attribute);

            var attributes = htmlAttributes.Parse(text);

            foreach(var attribute in attributes)
            {
                if (_isCanceled)
                {
                    return;
                }

                if (IsMatch(attribute))
                {
                    InternalOnDiscovered(attribute);
                }
            }
        }

        protected virtual void InternalOnDiscovered(string url)
        {
            OnDiscovered(Transform(url));
        }

        protected void Cancel()
        {
            _isCanceled = true;
        }

        #region helper methods
        private string Transform(string value)
        {
            string output;

            if (TryTransform(value, out output))
            {
                return output;
            }

            return value;
        }

        private bool TryTransform(string value, out string output)
        {
            output = string.Empty;

            if (_config.Transform == null ||
                string.IsNullOrEmpty(_config.Transform.Pattern) ||
                string.IsNullOrEmpty(_config.Transform.Expression))
            {
                return false;
            }

            var pattern = new Regex(_config.Transform.Pattern);

            var match = pattern.Match(value);

            if (!match.Success)
            {
                return false;
            }

            var groupValueList = new List<string>();

            foreach (Group group in match.Groups)
            {
                groupValueList.Add(group.Value);
            }

            try
            {
                output = string.Format(_config.Transform.Expression, groupValueList.ToArray());
            }
            catch
            {
                return false;
            }

            return true;
        }
        
        private bool IsMatch(string text)
        {
            if(string.IsNullOrEmpty(_config.Filter))
            {
                return true;
            }

            var pattern = new Regex(_config.Filter);

            return pattern.IsMatch(text);
        }
        #endregion
    }
}
