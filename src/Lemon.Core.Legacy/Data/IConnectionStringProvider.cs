using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core.Data
{
    public interface IConnectionStringProvider
    {
        /// <summary>
        /// Get the Connection String
        /// </summary>
        /// <param name="name">Connection Name</param>
        /// <returns></returns>
        string GetConnectionString(string name = null);

        /// <summary>
        /// Execute Dispose
        /// </summary>
        void Dispose();
    }
}