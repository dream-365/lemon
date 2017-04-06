using System;

namespace Lemon.Core
{
    public interface ISource
    {
        Type SourceType { get; }

        Node Next { get; set; }
    }
}
