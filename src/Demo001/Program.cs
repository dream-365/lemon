using Lemon.Transform;
using System.Collections.Generic;

namespace Demo001
{
    class Program
    {
        static void Main(string[] args)
        {
            LemonTransform.UseDefaultSevices();

            LemonTransform.InstallPackage<Package2>("package2");

            var engine = new CoreDocumentTransformEngine();

            engine.Execute("package2", new Dictionary<string, string> {
                {"scope", "uwp" }
            });
        }
    }
}
