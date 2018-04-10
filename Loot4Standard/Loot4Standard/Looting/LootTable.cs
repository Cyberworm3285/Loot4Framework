using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.Linq;

namespace Loot4Standard.Looting
{
    public class LootTable<T> : IEnumerable<KeyValuePair<int, T>>
    {
        private List<KeyValuePair<int, T>> _rarLootPairs;
        private List<KeyValuePair<int, T>> _sortedRarLootPairs;
        private Random _rand;

        private int _length;
        public int Length => _length;

        #region Constructors

        public LootTable()
        {
            _length = 0;
            _rand = new Random();
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

        #endregion

        public void Filter(Func<KeyValuePair<int, T>, bool> selector)
            => _sortedRarLootPairs = _rarLootPairs
                                        .Where(selector)
                                        .ToList();

        public T GetFilteredValue(int value) => GetValue(value, 0, _sortedRarLootPairs.Count - 1, ref _sortedRarLootPairs);

        public T GetValue(int value) => GetValue(value, 0, _rarLootPairs.Count - 1, ref _rarLootPairs);

        private T GetValue(int value, int min, int max, ref List<KeyValuePair<int, T>> items)
        {
            int piv = (min + max) / 2;
            if (value < items[piv].Key)
                if (piv == 0 || value >= items[piv - 1].Key)
                    return items[piv].Value;
                else
                    return GetValue(value, min, piv, ref items);
            else return GetValue(value, piv + 1, max, ref items);
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

        public void AddRange(IEnumerable<KeyValuePair<int, T>> en)
        {
            _rarLootPairs.AddRange(en);
        }

        /// <summary>
        /// Bad performance => avoid
        /// </summary>
        public int RemoveIf(Func<KeyValuePair<int, T>, bool> selector)
        {
            int c = 0;
            int deleted = 0;
            for (int i = 0; i < _rarLootPairs.Count;)
            {
                if (selector(_rarLootPairs[i]))
                {
                    _length -= _rarLootPairs[i].Key;
                    deleted += _rarLootPairs[i].Key;
                    _rarLootPairs.RemoveAt(i);
                    c++;
                }
                else
                {
                    i++;
                    _rarLootPairs[i] = new KeyValuePair<int, T>(_rarLootPairs[i].Key - deleted, _rarLootPairs[i].Value);
                }
            }
            return c;
        }

        #region IEnumerable

        IEnumerator<KeyValuePair<int, T>> IEnumerable<KeyValuePair<int, T>>.GetEnumerator() => this._rarLootPairs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this._rarLootPairs.GetEnumerator();

        #endregion

        public override string ToString() => $"LootTable containing <{typeof(T)}>";

        public T Next() => GetValue(_rand.Next(Length));

        public IEnumerable<T> Generate()
        {
            while (true)
                yield return Next();
        }

        public IEnumerable<T> Generate(int count)
        {
            for (var i = 0; i < count; i++)
                yield return Next();
        }
    }
}
