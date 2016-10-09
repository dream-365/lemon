using Lemon.Data.Core.Models;
using System.Linq;
using System.Text;

namespace Lemon.Data.Core
{
    public class SqlQueryBuilder
    {
        private string _tableName;

        private string[] _primaryKeys;

        public SqlQueryBuilder(string tableName, string[] primaryKeys)
        {
            _tableName = tableName;

            _primaryKeys = primaryKeys;
        }

        public string Build(DataTableSchema schema, bool upsert = true)
        {
            var sb = new StringBuilder();

            sb.Append("IF NOT EXISTS(SELECT * FROM [dbo].")
                .Append("[" + _tableName + "]");

            var firstPrimaryKey = _primaryKeys.FirstOrDefault();

            sb.Append(" WHERE [").Append(firstPrimaryKey).Append("] = @").Append(firstPrimaryKey);

            foreach (var primaryKey in _primaryKeys.Skip(1))
            {
                sb.Append(" AND ").Append("[").Append(primaryKey).Append("] = @").Append(primaryKey);
            }

            sb.AppendLine(")");

            sb.Append("INSERT INTO [dbo].")
                .Append("[" + _tableName + "]")
                .Append(" (")
                .Append(string.Join(",", schema.Columns.Select(m => string.Format("[{0}]", m)))).AppendLine(")");

            sb.Append("VALUES ")
                .Append("(")
                .Append(string.Join(",", schema.Columns.Select(m => string.Format("@{0}", m)))).AppendLine(")");

            if (upsert)
            {
                sb.AppendLine("ELSE");

                sb.Append("UPDATE [dbo].").AppendLine("[" + _tableName + "]");

                var sets = schema.Columns.Skip(1).Select(m => string.Format("[{0}] = @{0}", m));

                sb.Append("SET ").AppendLine(string.Join(",", sets));

                sb.Append(" WHERE [").Append(firstPrimaryKey).Append("] = @").Append(firstPrimaryKey);

                foreach (var primaryKey in _primaryKeys.Skip(1))
                {
                    sb.Append(" AND ").Append("[").Append(primaryKey).Append("] = @").Append(primaryKey);
                }
            }

            return sb.ToString();
        }
    }
}
