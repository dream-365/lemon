using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core
{
    public interface IMessageQueue
    {
        bool Send<T>(T body);

        T Dequeue<T>();

        void Close();
    }
}
