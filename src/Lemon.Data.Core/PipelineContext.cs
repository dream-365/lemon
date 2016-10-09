using System.Collections.Generic;

namespace Lemon.Data.Core
{
    /// <summary>
    /// Pipeline context: provide the context information for data pipeline
    /// </summary>
    public class PipelineContext 
    {
        private IDictionary<string, object> _namedParameters = new Dictionary<string, object>();

        private ProgressIndicator _progressIndicator;

        public ProgressIndicator ProgressIndicator { get { return _progressIndicator; } }

        public PipelineContext(ProgressIndicator progressIndicator, IDictionary<string, object> namedParameters = null)
        {
            if(namedParameters != null)
            {
                _namedParameters = namedParameters;
            }

            _progressIndicator = progressIndicator;
        }

        /// <summary>
        /// Get the named parameter value
        /// </summary>
        /// <param name="key">key to access the parameter value</param>
        /// <returns></returns>
        public object GetNamedParameterValue(string key)
        {
            object value;

            if(_namedParameters.TryGetValue(key, out value))
            {
                return value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Set the named parameter value
        /// </summary>
        /// <param name="key">key to set</param>
        /// <param name="value">value to set</param>
        public void SetNamedParameterValue(string key, object value)
        {
            _namedParameters[key] = value;
        }
    }
}
