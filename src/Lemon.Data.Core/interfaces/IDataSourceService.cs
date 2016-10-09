namespace Lemon.Data.Core
{
    public interface IDataSourceService
    {
        DataInputModel GetDataInput(string name);

        DataOutputModel GetDataOutput(string name);
    }
}
