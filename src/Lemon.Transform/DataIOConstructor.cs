using System;

namespace Lemon.Transform
{
    public class DataIOConstructor
    {
        public AbstractDataInput CreateDataInput(DataInputModel model)
        {
            return GlobalConfiguration
                    .TransformConfiguration
                    .Container.Resolve<AbstractDataInput>(
                model.SourceType + "_input", 
                new { model = model });
        }

        public AbstractDataOutput CreateDataOutput(DataOutputModel model)
        {
            return GlobalConfiguration
                .TransformConfiguration
                .Container.Resolve<AbstractDataOutput>(
                model.TargetType + "_output",
                new { model = model });
        }
    }
}
