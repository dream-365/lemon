using System;
using System.Collections.Generic;

namespace Lemon.Transform
{
    public class DataRowPipeline
    {
        public IList<TransformAction> Actions;

        private Action<BsonDataRow> _internalInput;

        public DataRowPipeline()
        {
            Actions = new List<TransformAction>();
        }

        public void Link()
        {
            if(Actions.Count < 1)
            {
                return;
            }

            _internalInput = Actions[0].Input;

            for (int i = 0; i < Actions.Count - 1; i++)
            {
                Actions[i].Output = Actions[i + 1].Input;

                Actions[i + 1].InputColumnNames = Actions[i].OutputColumnNames;
            }

            Actions[Actions.Count - 1].Output = Output;
        }

        public Action<BsonDataRow> Output;

        public void Input(BsonDataRow row)
        {
            if(_internalInput != null)
            {
                _internalInput(row);
            }
        }
    }
}
