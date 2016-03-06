using System;

namespace Lemon.Core
{
    // decrepted
    public interface IDiscover
    {
        event Action<string> OnDiscovered;

        void Start();
    }
}
