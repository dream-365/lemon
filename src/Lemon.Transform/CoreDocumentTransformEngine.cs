using System.Collections.Generic;

namespace Lemon.Transform
{
    public class CoreDocumentTransformEngine
    {
        private DataInputOutputFactory _dataFactory;

        public CoreDocumentTransformEngine()
        {
            _dataFactory = new DataInputOutputFactory();
        }

        public void Execute(string packageName, IDictionary<string, string> namedParameters = null)
        {
            var package = LemonTransform.PackageContainer.Resove(packageName);

            Execute(package, namedParameters);
        }

        public void Execute(TransformPackage package, IDictionary<string, string> namedParameters = null)
        {
            if(namedParameters != null)
            {
                package.Input.RepalceWithNamedParameters(namedParameters);

                package.Output.RepalceWithNamedParameters(namedParameters);
            }

            var input = _dataFactory.CreateDataInput(package.Input);

            var output = _dataFactory.CreateDataOutput(package.Output);

            var pipeline = new DataRowPipeline();

            pipeline.Actions = package.Actions;

            input.Output = pipeline.Input;

            pipeline.Output = output.Input;

            pipeline.Link();

            input.Start();
        }
    }
}
