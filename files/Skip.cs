using System;
using System.Collections.Generic;

namespace Curran.Utils
{
    static public class Skip
    {
        public class SkipFirst<T> : IEnumerable<T>
        {
            private IEnumerable<T> mEnum;
            public SkipFirst(IEnumerable<T> enm)
            {
                mEnum = enm;
            }
            #region IEnumerable<T> Members

            public IEnumerator<T> GetEnumerator()
            {
                IEnumerator<T> iter = mEnum.GetEnumerator();
                if (iter.MoveNext())
                {
                    while (iter.MoveNext())
                    {
                        yield return iter.Current;
                    }
                }
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        public class SkipLast<T> : IEnumerable<T>
        {
            private IEnumerable<T> mEnum;
            public SkipLast(IEnumerable<T> enm)
            {
                mEnum = enm;
            }
            #region IEnumerable<T> Members

            public IEnumerator<T> GetEnumerator()
            {
                IEnumerator<T> iter = mEnum.GetEnumerator();
                if (iter.MoveNext())
                {
                    T curr = iter.Current;
                    while (iter.MoveNext())
                    {
                        yield return curr;
                        curr = iter.Current;
                    }
                }

            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        static public SkipFirst<T> First<T>(IEnumerable<T> enm)
        {
            return new SkipFirst<T>(enm);
        }
        static public SkipLast<T> Last<T>(IEnumerable<T> enm)
        {
            return new SkipLast<T>(enm);
        }

    }
}
