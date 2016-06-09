namespace Lemon.Transform
{
    public class CoreDocumentTransformEngine2
    {
        private DataInputOutputFactory _dataFactory;

        public CoreDocumentTransformEngine2()
        {
            _dataFactory = new DataInputOutputFactory();
        }

        public void Execute(SessionContext context)
        {
            var dataReader = context.DataReader;

            var dataWritter = context.DataWritter;

            var pipeline = new DataRowPipeline();

            pipeline.Actions.Add(null);

            pipeline.Output = (row) => {
                dataWritter.Write(row);
            };

            dataReader.ForEach((dataRow) => {
                pipeline.Input(dataRow);
            });
        }

        public void Execute(string packageName)
        {
            var package = LemonTransform.PackageContainer.Resove(packageName);

            Execute(package);
        }

        public void Execute(TransformPackage package)
        {
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
