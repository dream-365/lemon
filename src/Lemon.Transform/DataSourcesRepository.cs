namespace Lemon.Transform
{
    public class DataSourcesRepository
    {
        private IDataSourcesRepository  _service;

        public DataSourcesRepository()
        {
            _service = GlobalConfiguration
                .TransformConfiguration
                .Container.Resolve<IDataSourcesRepository>();
        }

        public DataInputModel GetDataInput(string name)
        {
            return _service.GetDataInput(name);
        }

        public DataOutputModel GetDataOutput(string name)
        {
            return _service.GetDataOutput(name);
        }
    }
}
