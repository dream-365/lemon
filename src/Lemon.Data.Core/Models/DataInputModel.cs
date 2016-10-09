using Lemon.Data.Core.Models;

namespace Lemon.Data.Core
{
    public class DataInputModel
    {
        public string Name { get; set; }

        public string Object { get; set; }

        public DataConnection Connection { get; set; }

        public DataTableSchema Schema {get; set;}

        public string Filter { get; set; }

        public NamedParameter[] Parameters { get; set; }
    }
}
