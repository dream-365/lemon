using System;

namespace Lemon.Transform
{
    public class DataInputOutputFactory
    {
        public IDataInput CreateDataInput(DataInputModel model)
        {
            return LemonTransform.Container.Resolve<IDataInput>(
                model.SourceType + "_input", 
                new { model = model });
        }

        public IDataOutput CreateDataOutput(DataOutputModel model)
        {
            return LemonTransform.Container.Resolve<IDataOutput>(
                model.TargetType + "_output",
                new { model = model });
        }
    }
}
