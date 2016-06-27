using System;
using Lemon.Transform.Models;

namespace Lemon.Transform.Tests
{
    public class FakeDataOutput : AbstractDataOutput
    {
        protected override void OnReceive(BsonDataRow row)
        {
            
        }
    }
}
