﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Loot4Standard.Extensions
{
    public static class LINQ_Extensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> en, Action<T> action)
        {
            foreach (var x in en)
            {
                action(x);
                yield return x;
            }
        }
    }
}
