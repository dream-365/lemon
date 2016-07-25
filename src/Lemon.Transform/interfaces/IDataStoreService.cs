namespace Lemon.Transform
{
    public interface IDataSourcesRepository
    {
        DataInputModel GetDataInput(string name);

        DataOutputModel GetDataOutput(string name);
    }
}
