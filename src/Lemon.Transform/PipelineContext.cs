using System.Collections.Generic;

namespace Lemon.Transform
{
    /// <summary>
    /// Pipeline context: provide the context information for data pipeline
    /// </summary>
    public class PipelineContext
    {
        private IDictionary<string, string> _namedParameters = new Dictionary<string, string>();

        private ProgressIndicator _progressIndicator;

        private DataIOProvider _dataIOProvider;

        public DataIOProvider IO { get { return _dataIOProvider; } }

        public ProgressIndicator ProgressIndicator { get { return _progressIndicator; } }

        public PipelineContext(ProgressIndicator progressIndicator, IDictionary<string, string> namedParameters = null)
        {
            if(namedParameters != null)
            {
                _namedParameters = namedParameters;
            }

            _dataIOProvider = new DataIOProvider(this, _namedParameters);

            _progressIndicator = progressIndicator;
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

        /// <summary>
        /// Set the named parameter value
        /// </summary>
        /// <param name="key">key to set</param>
        /// <param name="value">value to set</param>
        public void SetNamedParameterValue(string key, string value)
        {
            _namedParameters[key] = value;
        }

        /// <summary>
        /// attach the transform action to running context
        /// </summary>
        /// <param name="action"></param>
        /// <param name="name"></param>
        /// <returns></returns>

        public TransformSingleAction Attach(TransformSingleAction action, string name = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                action.Name = name;
            }

            action.Context = this;

            return action;
        }

        /// <summary>
        /// attach the transform action to running context
        /// </summary>
        /// <param name="action"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public TransformManyAction Attach(TransformManyAction action, string name = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                action.Name = name;
            }

            action.Context = this;

            return action;
        }
    }
}
