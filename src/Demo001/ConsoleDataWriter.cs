using Lemon.Transform;
using System;
using System.Threading.Tasks;

namespace LemonDemo
{
    public class ConsoleDataWriter : IDataWriter<BsonDataRow>
    {
        private int _delay;
        private string _name;

        public ConsoleDataWriter(string name, int delay = 0)
        {
            _delay = delay;

            _name = name;
        }

        private long _count = 0;

        void IDataWriter<BsonDataRow>.Write(BsonDataRow record)
        {
            if(_delay > 0)
            {
                Task.Delay(_delay).Wait();
            }

            var info = string.Format("{2}:[{0}]-{1}", _count, record.ToString(), _name);

            Console.WriteLine(info);

            _count++;
        }
    }
}
