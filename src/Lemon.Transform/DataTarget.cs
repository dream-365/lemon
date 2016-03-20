namespace Lemon.Transform
{
    public class DataTarget
    {
        public string TargetType { get; set; }

        public string Connection { get; set; }

        public string PrimaryKey { get; set; }

        public bool IsUpsert { get; set; }
    }
}
