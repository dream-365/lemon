using System;

namespace Lemon.Transform.Tests
{
    public class FakeTransformAction1 : BaseTransformAction
    {
        protected override void Build()
        {
            Copy("Id", "PropertyId");

            Transform("TransformField", (oldValue) => {
                return oldValue.AsString + "_";
            });

            Calculate("CalculateField", (datarow) => {
                return datarow.GetValue("Id").AsString  + "_" + datarow.GetValue("TransformField").AsString;
            });
        }
    }
}
