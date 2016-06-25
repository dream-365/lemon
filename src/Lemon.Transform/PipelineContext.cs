using System.Collections.Generic;

namespace Lemon.Transform
{
    /// <summary>
    /// Pipeline context: provide the context information for data pipeline
    /// </summary>
    public class PipelineContext
    {
        private IDictionary<string, string> _namedParameters;

        private DataIOProvider _dataIOProvider;

        public DataIOProvider IO { get { return _dataIOProvider; } }

        public PipelineContext(IDictionary<string, string> namedParameters = null)
        {
            _namedParameters = namedParameters;

            _dataIOProvider = new DataIOProvider(_namedParameters);
        }

        /// <summary>
        /// Get the named parameter value
        /// </summary>
        /// <param name="key">key to access the parameter value</param>
        /// <returns></returns>
        public string GetNamedParameterValue(string key)
        {
            string value;

            if(_namedParameters.TryGetValue(key, out value))
            {
                return value;
            }

            return string.Empty;
        }
    }
}
