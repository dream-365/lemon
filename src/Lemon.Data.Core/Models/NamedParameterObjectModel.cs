using System.Collections.Generic;

namespace Lemon.Transform.Models
{
    public abstract class NamedParameterObjectModel
    {
        public abstract void RepalceWithNamedParameters(IDictionary<string, string> parameters);

        protected string RepalceWithNamedParameters(string text, IDictionary<string, string> parameters)
        {
            foreach (var placeholder in parameters)
            {
                var expression = "{{" + placeholder.Key + "}}";

                var replacement = placeholder.Value;

                text = text.Replace(expression, replacement);
            }

            return text;
        }
    }
}
