using System;
using System.Configuration;
using System.IO;

namespace Lemon.Transform
{
    public class SqlNamedQueryProvider
    {
        public static SqlNamedQueryProvider Instance = new SqlNamedQueryProvider();

        private string _root;

        private SqlNamedQueryProvider()
        {
            _root = ConfigurationManager.AppSettings["sql:root"];
        }

        public string Get(string name)
        {
            try
            {
                var filePath = Path.Combine(_root, name + ".sql");

                var text = File.ReadAllText(filePath);

                return text;
            }
            catch (FileNotFoundException)
            {
                throw new Exception("No SQL Statement found");
            }
        }
    }
}
