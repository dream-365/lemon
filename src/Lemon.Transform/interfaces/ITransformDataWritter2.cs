using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public interface ITransformDataWritter2
    {
        string PrimaryKey { get; }

        IEnumerable<DataColumn> DataColumns { get; set; }

        void Write(BsonDataRow row);
    }
}
