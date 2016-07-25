using System;

namespace Lemon.Transform.Models
{
    public class DataRowTransformWrapper<TRow>
    {
        public TRow Row { get; set; }

        public bool Success { get; set; }

        public Exception Exception { get; set; }
    }
}
