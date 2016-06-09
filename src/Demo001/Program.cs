using Lemon.Transform;

namespace Demo001
{
    class Program
    {
        static void Main(string[] args)
        {
            LemonTransform.UseDefaultSevices();

            LemonTransform.InstallPackage<Package1>("package1");

            var engine = new CoreDocumentTransformEngine();

            engine.Execute("package1");
        }
    }
}
