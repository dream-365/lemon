using System;

namespace Lemon.Core
{
    public interface ITarget
    {
        Type TargetType { get; }

        Node Prev { get; set; }
    }
}
