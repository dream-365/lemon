namespace Lemon.Core.Models
{
    public class DataTableSchema
    {
        public string Name { get; set; }

        public string[] PrimaryKeys { get; set; }

        public DataColumn[] Columns { get; set; }
    }
}
