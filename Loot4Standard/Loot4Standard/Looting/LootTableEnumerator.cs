using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Loot4Standard.Looting
{
    class LootTableEnumerator<T> : IEnumerator<KeyValuePair<int, T>>
    {
        List<KeyValuePair<int, T>> _list;
        int counter = -1;

        public LootTableEnumerator(List<KeyValuePair<int, T>> list) => _list = list;

        object IEnumerator.Current => _list[counter];

        KeyValuePair<int, T> IEnumerator<KeyValuePair<int, T>>.Current => _list[counter];

        public void Dispose()
        {
           
        }

        public bool MoveNext()
        {
            if (++counter == _list.Count)
                return false;
            return true;
        }

        public void Reset()
        {
            counter = -1;
        }
    }
}
