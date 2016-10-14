using System;
using System.Collections;
using System.Collections.Generic;

namespace Lemon.Data.Core
{
    public class PageEnumerator<T> : IEnumerator<T>
    {
        private const int DEFAULT_LEN_OF_PAGE = 1024;

        private long _globalIndex;

        private int _currentIndex;

        private long _currentPageIndex;

        private T[] _data;

        private int _length;

        private bool _hasMore;

        private IEnumerator<T> _enumerator;

        public PageEnumerator(IEnumerator<T> enumerator, int lengthOfPage)
        {
            _enumerator = enumerator;

            _length = lengthOfPage;

            _hasMore = _enumerator.MoveNext(); ;

            Reset();
        }

        public T Current
        {
            get
            {
                return _data[_currentIndex];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            _globalIndex++;

            _currentIndex++;

            if (_currentIndex >= _data.Length)
            {
                if(_hasMore)
                {
                    _hasMore = LoadData();

                    _currentIndex = 0;

                    _currentPageIndex++;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private bool LoadData()
        {
            bool hasMore = true;

            lock(_data)
            {
                var list = new List<T>();

                for (int i = 0; i < _length; i++)
                {
                    if (!hasMore)
                    {
                        break;
                    }

                    list.Add(_enumerator.Current);

                    hasMore = _enumerator.MoveNext();
                }

                _data = list.ToArray();

                return hasMore;
            }
        }

        public void Reset()
        {
            _globalIndex = -1;

            _currentPageIndex = -1;

            _currentIndex = -1;

            _data = new T[] { };
        }
    }
}
