using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LemonDemo
{
    public class Messages
    {
        private long _length;

        public Messages(long length)
        {
            _length = length;
        }

        public IEnumerable<Message> AsEnumerable()
        {
            for(long i = 0; i < _length; i++)
            {
                yield return new Message
                {
                    Id = string.Format("id+{0}", i),
                    Name = string.Format("name+{0}", i),
                    Title = new StringBuilder().Append("When you use the yield keyword in a statement, you indicate that the method, operator, or get accessor in which it appears is an iterator. Using yield to define an iterator removes the need for an explicit extra class (the class that holds the state for an enumeration, see IEnumerator<T> for an example) when you implement the IEnumerable and IEnumerator pattern for a custom collection type.").ToString()
                };
            }
        }
    }
}
