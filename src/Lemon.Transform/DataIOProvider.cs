namespace Lemon.Transform
{
    public class DataIOProvider
    {
        private DataSourceService _repository;

        private DataIOConstructor _factory;

        private PipelineContext _context;

        public DataIOProvider(PipelineContext context)
        {
            _repository = new DataSourceService();

            _factory = new DataIOConstructor();

            _context = context;
        }

        public AbstractDataInput GetInput(string name)
        {
            var model = _repository.GetDataInput(name);

            var input = _factory.CreateDataInput(model);

            _context.Attach(input, name);

            return input;
        }

        public AbstractDataOutput GetOutput(string name)
        {
            var model = _repository.GetDataOutput(name);

            var output  = _factory.CreateDataOutput(model);

            _context.Attach(output, name);

            return output;
        }
    }
}
