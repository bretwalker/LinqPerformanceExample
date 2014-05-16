using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LinqPerformanceExample
{
    class Program
    {
        private const int Loops = 10000;
        private const int Iterations = 2;

        private static void Main(string[] args)
        {
            var linqRuntimes = new List<double>();
            var dictRuntimes = new List<double>();

            var people = new List<Person>();
            for (var i = 0; i < 1000; i++)
            {
                people.Add(new Person(i.ToString()));
            }

            var stopwatch = new Stopwatch();

            for (var i = 0; i < Iterations; i++)
            {
                var random = new Random();
                people = people.OrderBy(p => random.Next()).ToList();

                // Linq
                stopwatch.Reset();
                stopwatch.Start();
                for (var l = 0; l < Loops; l++)
                {
                    // Each call is O(n)
                    // And it's common to see multiple Linq
                    // statements looking through a List in a single method
                    var person1 = people.Single(p => p.Name == "1");
                    var person2 = people.Single(p => p.Name == "2");
                }
                stopwatch.Stop();
                linqRuntimes.Add(stopwatch.Elapsed.TotalMilliseconds);

                // Dictionary
                stopwatch.Reset();
                stopwatch.Start();
                var peopleDict = people.ToDictionary(p => p.Name, p => p);
                for (var l = 0; l < Loops; l++)
                {
                    // Each call is O(1)
                    var person1 = peopleDict["1"];
                    var person2 = peopleDict["2"];
                }
                stopwatch.Stop();
                dictRuntimes.Add(stopwatch.Elapsed.TotalMilliseconds);
            }

            Console.Write("Linq average:\t");
            Console.WriteLine(linqRuntimes.Average());
            Console.Write("Dict average:\t");
            Console.WriteLine(dictRuntimes.Average());
            Console.WriteLine();
            Console.ReadKey();
        }
    }

    internal class Person
    {
        public string Name { get; set; }

        public Person(string name)
        {
            Name = name;
        }
    }
}
