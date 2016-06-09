using System;

namespace Lemon.Transform
{
    public class DataStore
    {
        private IDataStoreService  _service;

        public DataStore()
        {
            _service = LemonTransform.Container.Resolve<IDataStoreService>();
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
