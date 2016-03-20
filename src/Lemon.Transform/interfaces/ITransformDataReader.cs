using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public interface ITransformDataReader
    {
        void ForEach(Action<IValueProvider> forEach);

        string PrimaryKey { get; }
    }
}
