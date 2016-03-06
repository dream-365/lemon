using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Lemon.Core.Discover
{
    public class XPathFindUri
    {
        private XPathFindUriSetting _setting;

        public XPathFindUri(XPathFindUriSetting setting)
        {
            _setting = setting;
        }

        public IEnumerable<string> Find(string text)
        {
            var htmlAttributes = new HtmlAttributeParse(_setting.XPathAttributeMap.XPath, _setting.XPathAttributeMap.Attribute);

            var attributes = htmlAttributes.Parse(text);

            var findResult = new List<string>();

            foreach (var attribute in attributes)
            {
                if (IsMatch(attribute))
                {
                    findResult.Add(Transform(attribute));
                }
            }

            return findResult;
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

            if (_setting.Transform == null ||
                string.IsNullOrEmpty(_setting.Transform.Pattern) ||
                string.IsNullOrEmpty(_setting.Transform.Expression))
            {
                return false;
            }

            var pattern = new Regex(_setting.Transform.Pattern);

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
                output = string.Format(_setting.Transform.Expression, groupValueList.ToArray());
            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool IsMatch(string text)
        {
            if (string.IsNullOrEmpty(_setting.Filter))
            {
                return true;
            }

            var pattern = new Regex(_setting.Filter);

            return pattern.IsMatch(text);
        }
        #endregion
    }
}
