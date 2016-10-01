namespace Lemon.Transform
{
    public class DataSourceService : IDataSourceService
    {
        private IDataSourceService  _implService;

        internal IDataSourceService ImplService { get { return _implService; } }

        public DataSourceService()
        {
            _implService = GlobalConfiguration
                .TransformConfiguration
                .Container.Resolve<IDataSourceService>();
        }

        public DataInputModel GetDataInput(string name)
        {
            return _implService.GetDataInput(name);
        }

        public DataOutputModel GetDataOutput(string name)
        {
            return _implService.GetDataOutput(name);
        }
    }
}
