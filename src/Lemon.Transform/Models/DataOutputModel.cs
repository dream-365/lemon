using Lemon.Transform.Models;

namespace Lemon.Transform
{
    public class DataOutputModel
    {
        public virtual DataConnection Connection { get; set; }

        public string ConnectionId { get; set; }

        public virtual DataTableSchema Schema { get; set; }

        public string SchemaId { get; set; }

        /// <summary>
        /// false: insert-only
        /// true: insert-or-update
        /// </summary>
        public bool IsUpsert { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WriteOnChangeConfiguration WriteOnChange { get; set; }
    }
}
