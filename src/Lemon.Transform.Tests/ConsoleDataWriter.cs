using System;

namespace Lemon.Transform.Tests
{
    public class ConsoleDataWriter : IDataWriter<BsonDataRow>
    {
        private long _count = 0;

        void IDataWriter<BsonDataRow>.Write(BsonDataRow record)
        {
            var info = string.Format("[{0}]-{1}", _count, record.ToString());

            Console.WriteLine(info);

            _count++;
        }
    }
}
