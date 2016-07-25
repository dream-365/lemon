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
                model.Connection.ProviderName + "_input", 
                new { model = model });
        }

        public AbstractDataOutput CreateDataOutput(DataOutputModel model)
        {
            return GlobalConfiguration
                .TransformConfiguration
                .Container.Resolve<AbstractDataOutput>(
                model.Connection.ProviderName + "_output",
                new { model = model });
        }
    }
}
