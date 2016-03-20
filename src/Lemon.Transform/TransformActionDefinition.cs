namespace Lemon.Transform
{
    public class TransformActionDefinition
    {
        public DataSource Source { get; set; }

        public DataTarget Target { get; set; }

        public string Transformer { get; set; }
    }
}
