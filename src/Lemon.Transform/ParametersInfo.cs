using Lemon.Transform.Models;
using System;
using System.Collections.Generic;

namespace Lemon.Transform
{
    /// <summary>
    /// a class the store the pramters information
    /// </summary>
    public class ParametersInfo
    {
        private IDictionary<string, object> _registeredParametersWithDefaultValue = new Dictionary<string, object>();

        public void RegisterParameter(string name, object defaultValue = null)
        {
            _registeredParametersWithDefaultValue.Add(name, defaultValue);
        }

        /// <summary>
        /// set the default parameter value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetParameterDefultValue(string name, object value)
        {
            _registeredParametersWithDefaultValue[name] = value;
        }

        /// <summary>
        /// register prameters to parameter info with default values
        /// </summary>
        /// <param name="parameters"></param>
        public void RegisterParameters(IEnumerable<KeyValuePair<string, object>> parameters)
        {
            foreach(var parameter in parameters)
            {
                RegisterParameter(parameter.Key, parameter.Value);
            }
        }

        /// <summary>
        /// register prameters to parameter info with default values
        /// </summary>
        /// <param name="parameters"></param>
        public void RegisterParameters(IEnumerable<NamedParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                RegisterParameter(parameter.Name, parameter.DefaultValue == null 
                    ? null
                    : parameter.DefaultValue.Value);
            }
        }

        /// <summary>
        /// validate paramters to check if there is any missing paramters without default value
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public IDictionary<string, object> ValidateParameters(IDictionary<string, object> values)
        {
            if(values == null && _registeredParametersWithDefaultValue.Count > 0)
            {
                throw new ArgumentNullException("parameter(s) are required to execute");
            }

            var parameters = new Dictionary<string, object>();

            foreach(var registeredParameter in _registeredParametersWithDefaultValue)
            {
                object value;

                // fill the parameter if it exists
                if(values.TryGetValue(registeredParameter.Key, out value))
                {
                    parameters.Add(registeredParameter.Key, value);
                }
                // fall into the default value scenerio
                else if(registeredParameter.Value != null)
                {
                    parameters.Add(registeredParameter.Key, registeredParameter.Value);
                }
                // fall into the no parameter scenerio
                else
                {
                    throw new ArgumentException("cannot find the paramter named [" + registeredParameter.Key + "]");
                }
            }

            return parameters;
        }

        /// <summary>
        /// validate paramters to check if there is any missing paramters without default value
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public IDictionary<string, object> ValidateParameters(IDictionary<string, ValueObject> values)
        {
            IDictionary<string, object> dict = null;

            if (values != null)
            {
                dict = new Dictionary<string, object>();

                foreach(var value in values)
                {
                    dict.Add(value.Key, value.Value.Value);
                }
            }

            return ValidateParameters(dict);
        }
    }
}
