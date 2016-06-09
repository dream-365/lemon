namespace Lemon.Transform
{
    public class TransformActionDefinition
    {
        public DataInputModel Source { get; set; }

        public DataOutputModel Target { get; set; }

        public string Transformer { get; set; }
    }
}
