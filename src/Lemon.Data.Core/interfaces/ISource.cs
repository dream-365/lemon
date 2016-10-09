using System;

namespace Lemon.Data.Core
{
    public interface ISource
    {
        Type SourceType { get; }

        Node Next { get; set; }
    }
}
