namespace Lemon.Core
{
    public interface IStreamProcessingModuleProvider
    {
        IStreamProcessingModule Activate(string name);
    }
}
