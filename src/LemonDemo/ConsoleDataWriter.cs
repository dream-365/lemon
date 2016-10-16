using Lemon.Data.Core;
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
            if(_delay > 0)
            {
                Task.Delay(_delay).Wait();
            }

            _index++;

            if (_index % 5 == 0)
            {
                throw new Exception("write exception");
            }
            
            Console.WriteLine("[{2}]-{0}:{1}", _name, Newtonsoft.Json.JsonConvert.SerializeObject(record), _index);
        }

        public void Dispose()
        {
            
        }
    }
}
