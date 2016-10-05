using Lemon.Transform;
using System;
using System.Threading.Tasks;

namespace LemonDemo
{
    public class ConsoleDataWriter<TRecord> : IDataWriter<TRecord>
    {
        private int _delay;
        private string _name;

        public ConsoleDataWriter(string name, int delay = 0)
        {
            _delay = delay;

            _name = name;
        }

        public void Write(TRecord record)
        {
            Console.WriteLine(record);
        }
    }
}
