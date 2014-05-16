using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LinqPerformanceExample
{
    class Program
    {
        private const int Loops = 10000;
        private const int Iterations = 10;
        private const int ItemsToMap = 100;

        private static void Main(string[] args)
        {
            var linqRuntimes = new List<double>();
            var dictRuntimes = new List<double>();

            // Internal objects have no value. They will need to be mapped.
            var internalObjects = new List<InternalObject>();
            for (var i = 0; i < ItemsToMap; i++)
            {
                internalObjects.Add(new InternalObject {Key = i.ToString()});
            }

            var externalObjects = new List<ExternalObject>();
            for (var i = 0; i < ItemsToMap; i++)
            {
                externalObjects.Add(new ExternalObject {Key = i.ToString(), Value = i.ToString()});
            }

            var stopwatch = new Stopwatch();

            for (var i = 0; i < Iterations; i++)
            {
                var random = new Random();
                internalObjects = internalObjects.OrderBy(p => random.Next()).ToList();
                externalObjects = externalObjects.OrderBy(p => random.Next()).ToList();

                // Linq
                // Note: This isn't a LINQ perf issue, it's a consequence of a dev
                // choosing to put a O(n) operation in an O(n) operarion
                stopwatch.Reset();
                stopwatch.Start();
                for (var l = 0; l < Loops; l++)
                {
                    foreach (var internalObject in internalObjects)
                    {
                        internalObject.Value = externalObjects.First(o => o.Key == internalObject.Key).Value;
                    }
                }
                stopwatch.Stop();
                linqRuntimes.Add(stopwatch.Elapsed.TotalMilliseconds);

                // Dictionary
                stopwatch.Reset();
                stopwatch.Start();
                var externalObjectDict = externalObjects.ToDictionary(o => o.Key, o => o);
                for (var l = 0; l < Loops; l++)
                {
                    foreach (var internalObject in internalObjects)
                    {
                        internalObject.Value = externalObjectDict[internalObject.Key].Value;
                    }
                }
                stopwatch.Stop();
                dictRuntimes.Add(stopwatch.Elapsed.TotalMilliseconds);
            }

            Console.Write("Linq average:\t");
            Console.WriteLine(linqRuntimes.Average()); // 1193.32
            Console.Write("Dict average:\t");
            Console.WriteLine(dictRuntimes.Average()); // 52.10
            Console.WriteLine();
            Console.ReadKey();
        }
    }

    internal class InternalObject
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    internal class ExternalObject
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
