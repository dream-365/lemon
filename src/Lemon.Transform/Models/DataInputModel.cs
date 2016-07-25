using Lemon.Transform.Models;
using System.Collections.Generic;

namespace Lemon.Transform
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
