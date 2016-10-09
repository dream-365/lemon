using Lemon.Data.Core.Models;
using System.Linq;
using System.Text;

namespace Lemon.Data.Core
{
    public class SqlUtil
    {
        public static string BuildInsertSql(DataTableSchema schema, bool upsert = true)
        {
            var sb = new StringBuilder();

            sb.Append("IF NOT EXISTS(SELECT * FROM [dbo].")
                .Append("[" + schema.Name + "]");

            var firstPrimaryKey = schema.PrimaryKeys.FirstOrDefault();

            sb.Append(" WHERE [").Append(firstPrimaryKey).Append("] = @").Append(firstPrimaryKey);

            foreach (var primaryKey in schema.PrimaryKeys.Skip(1))
            {
                sb.Append(" AND ").Append("[").Append(primaryKey).Append("] = @").Append(primaryKey);
            }

            sb.AppendLine(")");

            sb.Append("INSERT INTO [dbo].")
                .Append("[" + schema.Name + "]")
                .Append(" (")
                .Append(string.Join(",", schema.Columns.Select(m => string.Format("[{0}]", m.Name)))).AppendLine(")");

            sb.Append("VALUES ")
                .Append("(")
                .Append(string.Join(",", schema.Columns.Select(m => string.Format("@{0}", m.Name)))).AppendLine(")");

            if (upsert)
            {
                sb.AppendLine("ELSE");

                sb.Append("UPDATE [dbo].").AppendLine("[" + schema.Name + "]");

                var sets = schema.Columns.Skip(1).Select(m => string.Format("[{0}] = @{0}", m.Name));

                sb.Append("SET ").AppendLine(string.Join(",", sets));

                sb.Append(" WHERE [").Append(firstPrimaryKey).Append("] = @").Append(firstPrimaryKey);

                foreach (var primaryKey in schema.PrimaryKeys.Skip(1))
                {
                    sb.Append(" AND ").Append("[").Append(primaryKey).Append("] = @").Append(primaryKey);
                }
            }

            return sb.ToString();
        }
    }
}
