using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public interface ITransformDataWritter
    {
        IValueSetter GetValueSetter(object id);

        string PrimaryKey { get; }

        void Flush();
    }
}
