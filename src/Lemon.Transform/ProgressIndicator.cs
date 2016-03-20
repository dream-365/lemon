using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    internal class ProgressIndicator
    {
        private int _unit;

        private int _counter = 0;

        private DateTime _start;

        private DateTime _end;

        public ProgressIndicator(int unit)
        {
            _start = DateTime.Now;

            _unit = unit;
        }

        public void Increment()
        {
            _counter = _counter + 1;


            if(_counter % _unit == 0)
            {
                Console.Write("#");
            }
        }

        public void Summary()
        {
            _end = DateTime.Now;

            Console.WriteLine();

            Console.WriteLine("Duration: {0}, total: {1}", _end - _start, _counter);
        }
    }
}
