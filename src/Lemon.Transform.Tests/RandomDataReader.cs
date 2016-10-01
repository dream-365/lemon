namespace Lemon.Transform.Tests
{
    public class RandomDataReader : IDataReader<BsonDataRow>
    {
        private long _index = 0;

        public bool End
        {
            get
            {
                return false;
            }
        }

        public BsonDataRow Read()
        {
            return new BsonDataRow(new MongoDB.Bson.BsonDocument {
                    {"id",  _index++}
                });
        }
    }
}
