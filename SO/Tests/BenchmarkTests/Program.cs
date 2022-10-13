using BenchmarkDotNet.Running;

namespace BenchmarkTests
{
    internal class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<RestBenchmarks>();
            Console.ReadKey();
        }
    }
}