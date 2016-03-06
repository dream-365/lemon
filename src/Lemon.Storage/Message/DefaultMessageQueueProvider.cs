using Lemon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Storage.Message
{
    public class DefaultMessageQueueProvider : IMessageQueueProvider
    {
        public IMessageQueue Get(string name, bool createIfNotExists)
        {
            return new CloudMessageQueue(name, createIfNotExists);
        }
    }
}
