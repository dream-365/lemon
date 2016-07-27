using System.Collections.Generic;

namespace Lemon.Transform
{
    /// <summary>
    /// Pipeline context: provide the context information for data pipeline
    /// </summary>
    public class PipelineContext
    {
        private IDictionary<string, object> _namedParameters = new Dictionary<string, object>();

        private ProgressIndicator _progressIndicator;

        private DataIOProvider _dataIOProvider;

        public DataIOProvider IO { get { return _dataIOProvider; } }

        public ProgressIndicator ProgressIndicator { get { return _progressIndicator; } }

        public PipelineContext(ProgressIndicator progressIndicator, IDictionary<string, object> namedParameters = null)
        {
            if(namedParameters != null)
            {
                _namedParameters = namedParameters;
            }

            _dataIOProvider = new DataIOProvider(this);

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
        public void SetNamedParameterValue(string key, string value)
        {
            _namedParameters[key] = value;
        }


        /// <summary>
        /// attach the link object to current context
        /// </summary>
        /// <typeparam name="TObject">link object type</typeparam>
        /// <param name="obj">object</param>
        /// <param name="name">specify the name in context</param>
        /// <returns>attached object</returns>
        public TObject Attach<TObject>(
            TObject obj,
            string name = null) where TObject : PipelineObject
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                obj.Name = name;
            }

            obj.Context = this;

            return obj;
        }
    }
}
