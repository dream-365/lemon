using System;

namespace Lemon.Core
{
    public interface IDiscover
    {
        event Action<string> OnDiscovered;

        void Start();
    }
}
