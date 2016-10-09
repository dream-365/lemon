using System;

namespace Lemon.Data.Core.Tests
{
    public class CustomDataSourceService : IDataSourceService
    {
        public DataInputModel GetDataInput(string name)
        {
            throw new NotImplementedException();
        }

        public DataOutputModel GetDataOutput(string name)
        {
            throw new NotImplementedException();
        }
    }
}
