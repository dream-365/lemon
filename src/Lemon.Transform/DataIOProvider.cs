using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public class DataIOProvider
    {
        private DataStore _store;

        private DataIOConstructor _factory;

        private IDictionary<string, string> _namedParameters;

        public DataIOProvider(IDictionary<string, string> namedParameters = null)
        {
            _store = new DataStore();

            _factory = new DataIOConstructor();

            _namedParameters = namedParameters;
        }

        public AbstractDataInput GetInput(string name)
        {
            var model = _store.GetDataInput(name);

            if (_namedParameters != null)
            {
                model.RepalceWithNamedParameters(_namedParameters);
            }

            return _factory.CreateDataInput(model);
        }

        public AbstractDataOutput GetOutput(string name)
        {
            var model = _store.GetDataOutput(name);

            if (_namedParameters != null)
            {
                model.RepalceWithNamedParameters(_namedParameters);
            }

            return _factory.CreateDataOutput(model);
        }
    }
}
