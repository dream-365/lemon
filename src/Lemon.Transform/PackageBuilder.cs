using System;
using System.Collections.Generic;

namespace Lemon.Transform
{
    public class PackageBuilder
    {
        private DataStore _dataStore;

        private string _inputName;

        private string _outputName;

        private IList<TransformAction> _actions;

        public PackageBuilder()
        {
            _dataStore = new DataStore();

            _actions = new List<TransformAction>();
        }

        public PackageBuilder Input(string name)
        {
            _inputName = name;

            return this;
        }

        public PackageBuilder Output(string name)
        {
            _outputName = name;

            return this;
        }

        public PackageBuilder Action(TransformAction action)
        {
            _actions.Add(action);

            return this;
        }

        public TransformPackage Build()
        {
            var input = _dataStore.GetDataInput(_inputName);

            var output = _dataStore.GetDataOutput(_outputName);

            var package = new TransformPackage
            {
                Input = input,
                Output = output,
                Actions = _actions
            };

            return package;
        }
    }
}
