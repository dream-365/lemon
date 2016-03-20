using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public interface IValueSetter
    {
        void SetValue(string name, BsonValue value);

        void Apply();
    }
}
