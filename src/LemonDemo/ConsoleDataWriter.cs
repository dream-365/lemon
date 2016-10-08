using Lemon.Transform;
using System;
using System.Threading.Tasks;

namespace LemonDemo
{
    public class ConsoleDataWriter<TRecord> : IDataWriter<TRecord>
    {
        private int _delay;
        private string _name;
        private long _index;

        public ConsoleDataWriter(string name, int delay = 0)
        {
            _delay = delay;

            _name = name;

            _index = 0;
        }

        public void Write(TRecord record)
        {
            Console.WriteLine("[{2}]-{0}:{1}", _name, record, _index++);
        }
    }
}
