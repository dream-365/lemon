using System.Data;

namespace Lemon.Core.Models
{
    public class DataColumn
    {
        public string Name { get; set; }
        
        public DbType DbType { get; set; }
    }
}
