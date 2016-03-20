﻿namespace Lemon.Transform
{
    public class CoreDocumentTransformEngine
    {
        public void Execute(TransformObject transformObject)
        {
            var dataReader = transformObject.DataReader;

            var dataWritter = transformObject.DataWritter;

            var transformColumnDefinitions = transformObject.TransformColumnDefinitions;

            var calculationColumnDefinition = transformObject.CalculationColumnDefinition;

            var progressIndicator = new ProgressIndicator(1000);

            dataReader.ForEach((provider) => {

                var id = provider.GetValue(dataReader.PrimaryKey).ToString();

                if (string.IsNullOrWhiteSpace(id))
                {
                    ConsoleUtilities.PrintErrorMessage("empty primary key error");

                    return;
                }

                var setter = dataWritter.GetValueSetter(id);

                foreach (var column in transformColumnDefinitions)
                {
                    if(column.TransformFunction != null)
                    {
                        var newVal = column.TransformFunction(provider.GetValue(column.SourceColumnName));

                        setter.SetValue(column.TargetColumnName, newVal);
                    }
                    else
                    {
                        setter.SetValue(column.TargetColumnName, provider.GetValue(column.SourceColumnName));
                    }
                }

                foreach(var column in calculationColumnDefinition)
                {
                    var calVal = column.CalculateFunction(provider);

                    setter.SetValue(column.TargetColumnName, calVal);
                }

                setter.Apply();

                progressIndicator.Increment();
            });

            dataWritter.Flush();

            progressIndicator.Summary();
        }
    }
}
