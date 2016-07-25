using Lemon.Transform.Models;

namespace Lemon.Transform
{
    public class DataInputModel
    {
        public string Id { get; set; }

        public DataConnection Connection { get; set; }

        public string ConnectionId { get; set; }

        public DataTableSchema Schema {get; set;}

        public string SchemaId { get; set; }

        public string Filter { get; set; }

        public NamedParameter[] Parameters { get; set; }
    }
}
