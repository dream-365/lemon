using Lemon.Transform;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Demo001
{
    public class RandomDataInput : AbstractDataInput
    {
        private const long DEFAULT_MAX = 1000;

        private long _max;

        public RandomDataInput(long? max = null)
        {
            _max = max ?? DEFAULT_MAX;
        }


        public override async Task StartAsync(IDictionary<string, object> parameters = null)
        {
            long index = 0;

            var text = "";

            for (int i = 0; i < 1024; i++)
            {
                text += i;
            }

            int len = text.Length;

            while (index < _max)
            {
                char[] characters = new char[len];

                text.CopyTo(0, characters, 0, len);

                var str = new string(characters);

                await SendAsync(new BsonDataRow(new MongoDB.Bson.BsonDocument {
                    {"id",  index},
                    {"text", str}
                }));

                index++;
            }

            Complete();
        }
    }
}
