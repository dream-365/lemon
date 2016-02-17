using System;
using System.Configuration;

namespace Lemon.Core.Data
{
    public class UniversalConnectionStringProvider : IConnectionStringProvider
    {
        private const string DEFAULT_CONNECTION_NAME = "DefaultConnection";

        public void Dispose()
        {
            
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name">mongo:uwp</param>
        /// <returns></returns>
        public string GetConnectionString(string name = null)
        {
            if(string.IsNullOrEmpty(name))
            {
                return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }

            if(name.StartsWith("mongo:"))
            {
                var cells = name.Split(':');

                var database = cells[1];

                var baseConnection = ConfigurationManager.ConnectionStrings["MongoDBConnection"].ConnectionString;

                return baseConnection + "/" + database;
            }

            var node = ConfigurationManager.ConnectionStrings[name];

            if (node == null)
            {
                throw new Exception(string.Format("the connection name {0} is not found", name));
            }

            return node.ConnectionString;
        }
    }
}
