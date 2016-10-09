using Lemon.Data.Core.Models;

namespace Lemon.Data.Core
{
    public class DataOutputModel
    {
        public string Name { get; set; }

        public string Object { get; set; }

        public DataConnection Connection { get; set; }

        public DataTableSchema Schema { get; set; }

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
