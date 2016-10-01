using Lemon.Transform;

namespace LemonDemo
{
    public class RandomDataReader : IDataReader<BsonDataRow>
    {
        private bool _end = false;

        private long _index = 0;

        private long _max = int.MaxValue;

        public bool End
        {
            get
            {
                return _end;
            }
        }

        public RandomDataReader(int max)
        {
            _max = max;
        }

        public BsonDataRow Read()
        {
            var row = new BsonDataRow(new MongoDB.Bson.BsonDocument {
                    {"id",  _index++}
                });

            _end = _index > _max;

            return row;
        }
    }
}
