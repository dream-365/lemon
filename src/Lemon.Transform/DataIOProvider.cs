using System.Collections.Generic;

namespace Lemon.Transform
{
    public class DataIOProvider
    {
        private DataStore _store;

        private DataIOConstructor _factory;

        private IDictionary<string, string> _namedParameters;

        private PipelineContext _context;

        public DataIOProvider(PipelineContext context, IDictionary<string, string> namedParameters = null)
        {
            _store = new DataStore();

            _factory = new DataIOConstructor();

            _namedParameters = namedParameters;

            _context = context;
        }

        public AbstractDataInput GetInput(string name)
        {
            var model = _store.GetDataInput(name);

            if (_namedParameters != null)
            {
                model.RepalceWithNamedParameters(_namedParameters);
            }

            var input = _factory.CreateDataInput(model);

            input.Context = _context;

            return input;
        }

        public AbstractDataOutput GetOutput(string name)
        {
            var model = _store.GetDataOutput(name);

            if (_namedParameters != null)
            {
                model.RepalceWithNamedParameters(_namedParameters);
            }

            var output  = _factory.CreateDataOutput(model);

            output.Context = _context;

            return output;
        }
    }
}
