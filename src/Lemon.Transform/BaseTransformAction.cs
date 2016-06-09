﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Lemon.Transform
{
    public abstract class BaseTransformAction : TransformAction
    {
        protected IList<TransformColumnDefinition> _transformColumnDefinitions;

        protected IList<CalculationColumnDefinition> _calculationColumnDefinitions;

        public BaseTransformAction()
        {
            _transformColumnDefinitions = new List<TransformColumnDefinition>();

            _calculationColumnDefinitions = new List<CalculationColumnDefinition>();

            Build();
        }

        protected abstract void Build();

        protected void Copy(string source, string target = null)
        {
            _transformColumnDefinitions.Add(new TransformColumnDefinition { SourceColumnName = source, TargetColumnName = target });
        }

        protected void Transform(string source, Func<BsonValue, BsonValue> func, string target = null)
        {
            _transformColumnDefinitions.Add(new TransformColumnDefinition { SourceColumnName = source, TargetColumnName = target, TransformFunction = func });
        }

        protected void Calculate(string name, Func<BsonDataRow, BsonValue> func)
        {
            _calculationColumnDefinitions.Add(new CalculationColumnDefinition { TargetColumnName = name, CalculateFunction = func });
        }


        public IEnumerable<TransformColumnDefinition> GetTransformColumnDefinitions()
        {
            return _transformColumnDefinitions;
        }

        public IEnumerable<CalculationColumnDefinition> GetCalculationColumnDefinitions()
        {
            return _calculationColumnDefinitions;
        }

        public override void Input(BsonDataRow inputRow)
        {
            var newRow = new BsonDataRow();

            foreach (var column in _transformColumnDefinitions)
            {
                if (column.TransformFunction != null)
                {
                    var newVal = column.TransformFunction(inputRow.GetValue(column.SourceColumnName));

                    newRow.SetValue(column.TargetColumnName, newVal);
                }
                else
                {
                    newRow.SetValue(column.TargetColumnName, inputRow.GetValue(column.SourceColumnName));
                }
            }

            foreach (var column in _calculationColumnDefinitions)
            {
                var calVal = column.CalculateFunction(inputRow);

                newRow.SetValue(column.TargetColumnName, calVal);
            }

            inputRow.CombineWith(newRow);

            Output(inputRow);
        }
    }
}