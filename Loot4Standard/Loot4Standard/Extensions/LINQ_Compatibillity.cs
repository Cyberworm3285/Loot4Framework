using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;

using Loot4Standard.Looting;

namespace Loot4Standard.Extensions
{
    public static class LINQ_Compatibillity
    {
        public static LootTable<T> ToLootTable<T>(this IEnumerable<KeyValuePair<int, T>> pairs) => new LootTable<T>(pairs);

        public static LootTable<T> ToLootTable<T>(this T[] values, int[] bounds) => new LootTable<T>(bounds, values);

        public static LootTable<T> ToLootTable<T>(this T[] values, Func<T, int> raritySelector) => new LootTable<T>(values.Select(raritySelector).ToArray(), values);
    }
}
