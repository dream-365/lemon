using System;

namespace Lemon.Transform
{
    public class DebugOutput : AbstractDataOutput
    {
        private long _count = 0;

        protected override void OnReceive(BsonDataRow row)
        {
            var info = string.Format("[{0}]-{1}", _count, row.ToString());

            Console.WriteLine(info);

            _count++; 
        }
    }
}
