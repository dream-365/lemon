using System;

namespace Lemon.Data.Core
{
    public interface ITarget
    {
        Type TargetType { get; }

        Node Prev { get; set; }
    }
}
