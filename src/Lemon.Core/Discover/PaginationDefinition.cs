namespace Lemon.Core.Discover
{
    public class RegexTransform
    {
        public string Pattern { get; set; }

        public string Expression { get; set; }
    }

    public class XPathAttributeMap
    {
        public string XPath { get; set; }
        public string Attribute { get; set; }
    }

    public class PaginationDefinition
    {
        public string Name { get; set; }

        public string UrlFormat { get; set; }

        public string BaseUri { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public string Encoding { get; set; }

        public string Filter { get; set; }

        public XPathAttributeMap LookUp { get; set; }

        public RegexTransform Transform { get; set; }

   }
}
