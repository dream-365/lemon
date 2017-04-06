using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lemon.Core
{
    public interface IExecuteable
    {
        Task<bool> RunAsync();
    }
}
