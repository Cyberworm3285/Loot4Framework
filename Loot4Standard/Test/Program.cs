using System;
using System.Linq;
using Loot4Standard.Looting;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            var leg = new LootTable<string>
            {
                { 2, "leg 1" },
                { 3, "leg 2" },
            };
            var rare = new LootTable<string>
            {
                { 41, "rare 1" },
                { 50, "rare 2" },
                { 40, "rare 3" },
            };
            var normal = new LootTable<string>
            {
                { 400, "normal-crap" },
            };

            var masterTable = new LootTable<LootTable<string>>
            {
                { 200, leg },
                { 800, rare },
                { 4000, normal },
            };

            var dicNames = new Dictionary<LootTable<string>, string>
            {
                { leg, "legendary" },
                { rare, "rare" },
                { normal, "normal" },
            };

            var rand = new Random();
            var rarCounts = dicNames.Values.ToDictionary(x => x, x => 0);

            for (int i = 0; i < 5000000; i++)
            {
                var rarity = masterTable[rand.Next(masterTable.Length)];
                rarCounts[dicNames[rarity]]++;
                //var item = rarity[rand.Next(rarity.Length)];
                //Console.WriteLine($"{dicNames[rarity]} :: {item}");
            }

            foreach (var x in masterTable.Select(y => $"{y.Key} :: {y.Value}"))
            {
                Console.WriteLine(x);
            }

            Console.ReadKey();
        }
    }
}