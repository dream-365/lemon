using Lemon.Transform;

namespace Demo001
{
    public class DummyOutput : AbstractDataOutput
    {
        protected override void OnReceive(BsonDataRow row)
        {
            System.Console.WriteLine("dummy: " + row.GetValue("id"));
        }
    }
}
