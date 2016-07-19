using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Transform
{
    internal class SQLInserOrUpdateQueryBuilder
    {
        private string _tableName;

        private string _primaryKey;

        public SQLInserOrUpdateQueryBuilder(string tableName, string primaryKey)
        {
            _tableName = tableName;

            _primaryKey = primaryKey;
        }

        public string Build(IEnumerable<string> columns, bool upsert = true)
        {
            var sb = new StringBuilder();

            sb.Append("IF NOT EXISTS(SELECT * FROM [dbo].")
                .Append("[" + _tableName + "]").Append(" WHERE [").Append(_primaryKey).Append("] = @").Append(_primaryKey).AppendLine(")");

            sb.Append("INSERT INTO [dbo].")
                .Append("[" + _tableName + "]")
                .Append(" (")
                .Append(string.Join(",", columns.Select(m => string.Format("[{0}]", m)))).AppendLine(")");

            sb.Append("VALUES ")
                .Append("(")
                .Append(string.Join(",", columns.Select(m => string.Format("@{0}", m)))).AppendLine(")");


            if (upsert)
            {
                sb.AppendLine("ELSE");

                sb.Append("UPDATE [dbo].").AppendLine("[" + _tableName + "]");

                var sets = columns.Skip(1).Select(m => string.Format("[{0}] = @{0}", m));

                sb.Append("SET ").AppendLine(string.Join(",", sets));

                sb.Append("WHERE [").Append(_primaryKey).Append("] = @").AppendLine(_primaryKey);
            }

            return sb.ToString();
        }
    }
}
