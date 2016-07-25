using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lemon.Transform;

namespace Demo001
{
    public class ConsoleOutput : AbstractDataOutput
    {
        private string[] _columnNames;

        private int _count = 0;

        public ConsoleOutput(DataOutputModel model)
        {
            _columnNames = model.Schema.ColumnNames.ToArray();
        }

        public void Input(BsonDataRow inputRow)
        {
            foreach (var columnName in _columnNames)
            {
                Console.Write(_count + "|" + inputRow.GetValue(columnName).ToString() + "|");
            }

            _count++;

            Console.WriteLine();
        }

        protected override void OnReceive(BsonDataRow row)
        {
            Input(row);
        }
    }
}
