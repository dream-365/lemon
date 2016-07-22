using System;
using Lemon.Transform.Models;

namespace Lemon.Transform.Tests
{
    public class FakeDataOutput : AbstractDataOutput
    {
        protected override DataRowStatusContext BuildDataRowStatusContext(string[] excludes)
        {
            throw new NotImplementedException();
        }

        protected override void OnReceive(BsonDataRow row)
        {
            
        }
    }
}
