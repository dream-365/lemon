using System;

namespace Lemon.Transform
{
    public interface ITarget
    {
        Type TargetType { get; }

        Node Prev { get; set; }
    }
}
