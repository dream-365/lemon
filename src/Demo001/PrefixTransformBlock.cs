using Lemon.Transform;
using Lemon.Transform.Models;

namespace LemonDemo
{
    public class PrefixTransformBlock : ITransformBlock
    {
        private string _prefix;

        public PrefixTransformBlock(string prefix)
        {
            _prefix = prefix;
        }

        public DataRowTransformWrapper<BsonDataRow> Transform(DataRowTransformWrapper<BsonDataRow> data)
        {
            var prefix = data.Row.GetValue("prefix");

            if(prefix == MongoDB.Bson.BsonNull.Value)
            {
                data.Row.SetValue("prefix", _prefix);
            }else if(prefix.BsonType == MongoDB.Bson.BsonType.String)
            {
                data.Row.SetValue("prefix", _prefix + ":" + prefix.AsString);
            }

            return data;
        }
    }
}
