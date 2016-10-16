using System;
using System.Collections.Generic;
using System.Linq;
using Lemon.Data.Core.Models;
using System.Text;

namespace Lemon.Data.IO
{
    internal class Util
    {
        public static DataTableSchema BuildSchemaFromType(Type type)
        {
            var columns = new List<DataColumn>();

            var primaryKeys = new List<string>();

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var notMapped = property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute), false)
                                        .FirstOrDefault();

                if (notMapped != null)
                {
                    continue;
                }

                columns.Add(new DataColumn { Name = property.Name });

                var key = property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), false)
                                  .FirstOrDefault();

                if (key != null)
                {
                    primaryKeys.Add(property.Name);
                }
            }

            return new DataTableSchema
            {
                Columns = columns.ToArray(),
                PrimaryKeys = primaryKeys.ToArray()
            };
        }

        public static string BuildSelectSql(DataTableSchema schema, string whereCaluse = null)
        {
            var sb = new StringBuilder();

            sb.Append("SELECT");

            sb.AppendFormat(" [{0}]", schema.Columns.FirstOrDefault().Name);

            foreach(var column in schema.Columns.Skip(1))
            {
                sb.AppendFormat(",[{0}]\r\n", column.Name);
            }

            sb.AppendFormat("FROM [{0}]\r\n", schema.Name);

            if(!string.IsNullOrWhiteSpace(whereCaluse))
            {
                sb.Append("WHERE " + whereCaluse);
            }           

            return sb.ToString();
        }

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
