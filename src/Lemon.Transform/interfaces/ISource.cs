using System;

namespace Lemon.Transform
{
    public interface ISource
    {
        Type SourceType { get; }

        Node Next { get; set; }
    }
}
