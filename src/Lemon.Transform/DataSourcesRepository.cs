namespace Lemon.Transform
{
    public class DataSourcesRepository
    {
        private IDataStoreService  _service;

        public DataSourcesRepository()
        {
            _service = GlobalConfiguration
                .TransformConfiguration
                .Container.Resolve<IDataStoreService>();
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
