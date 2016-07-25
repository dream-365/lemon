using System.Collections.Generic;

namespace Lemon.Transform.Models
{
    public class DataTableSchema
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<DataColumn> Columns { get; set; }
    }
}
