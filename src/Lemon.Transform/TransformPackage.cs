using System.Collections.Generic;

namespace Lemon.Transform
{
    public class TransformPackage
    {
        public DataInputModel Input;

        public DataOutputModel Output;

        public IList<TransformAction> Actions;
    }
}
