using System.Collections.Generic;

namespace Lemon.Transform
{
    public class IOContext
    {
        private DataStore _store;

        private DataInputOutputFactory _factory;

        private IDictionary<string, string> _namedParameters;

        public IOContext(IDictionary<string, string> namedParameters = null)
        {
            _store = new DataStore();

            _factory = new DataInputOutputFactory();

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
