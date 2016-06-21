using Lemon.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo001
{
    public class BadDataAction : TransformSingleAction
    {
        public override BsonDataRow Transform(BsonDataRow row)
        {
            var name = row.GetValue("name");

            if(name == "data001")
            {
                return null;
            }

            return row;
        }
    }
}
