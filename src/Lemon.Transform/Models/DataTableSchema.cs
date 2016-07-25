namespace Lemon.Transform.Models
{
    public class DataTableSchema
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ObjectName { get; set; }

        public string[] ColumnNames { get; set; }

        public string[] PrimaryKeys { get; set; }
    }
}
