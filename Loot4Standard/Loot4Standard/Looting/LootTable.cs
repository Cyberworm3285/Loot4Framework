using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Loot4Standard.Looting
{
    public class LootTable<T> : IEnumerable<KeyValuePair<int, T>>
    {
        private List<KeyValuePair<int, T>> _rarLootPairs;

        private int _length;
        public int Length => _length;

        public LootTable()
        {
            _length = 0;
            _rarLootPairs = new List<KeyValuePair<int, T>>();
        }

        public LootTable(int[] bounds, T[] content) : this()
        {
            if (content.Length != bounds.Length)
                throw new ArgumentException("Length mismatch ma boi");

            for (var i = 0; i < bounds.Length; i++)
            {
                _length += bounds[i];
                _rarLootPairs.Add(new KeyValuePair<int, T>(_length, content[i]));
            }
        }

        public LootTable(IEnumerable<KeyValuePair<int, T>> dic) : this()
        {
            foreach(var keyValue in dic)
            {
                _length += keyValue.Key;
                _rarLootPairs.Add(new KeyValuePair<int, T>(_length, keyValue.Value));
            }
        }

        public T GetValue(int value) => GetValue(value, 0, _rarLootPairs.Count - 1);

        private T GetValue(int value, int min, int max)
        {
            int piv = (min + max) / 2;
            if (value < _rarLootPairs[piv].Key)
                if (piv == 0 || value >= _rarLootPairs[piv - 1].Key)
                    return _rarLootPairs[piv].Value;
                else
                    return GetValue(value, min, piv);
            else return GetValue(value, piv + 1, max);
        }

        public T this[int value] => GetValue(value);

        public void Add(KeyValuePair<int, T> keyVal)
        {
            _length += keyVal.Key;
            _rarLootPairs.Add(new KeyValuePair<int, T>(_length, keyVal.Value));
        }

        public void Add(int rarity, T value)
        {
            _length += rarity;
            _rarLootPairs.Add(new KeyValuePair<int, T>(_length, value));
        }

        public int RemoveIf(Func<KeyValuePair<int, T>, bool> selector)
        {
            int c = 0;
            for (int i = 0; i < _rarLootPairs.Count;)
            {
                if (selector(_rarLootPairs[i]))
                {
                    _length -= _rarLootPairs[i].Key;
                    _rarLootPairs.RemoveAt(i);
                    c++;
                }
                else
                    i++;
            }
            return c;
        }

        #region IEnumerable

        IEnumerator<KeyValuePair<int, T>> IEnumerable<KeyValuePair<int, T>>.GetEnumerator() => new LootTableEnumerator<T>(_rarLootPairs);

        IEnumerator IEnumerable.GetEnumerator() => new LootTableEnumerator<T>(_rarLootPairs);

        #endregion

        public override string ToString() => $"LootTable containing <{typeof(T)}>";
    }
}
