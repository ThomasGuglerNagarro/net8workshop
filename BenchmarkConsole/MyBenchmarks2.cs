using BenchmarkDotNet.Attributes;
using ClassLibrary1;

namespace BenchmarkConsole;

[MemoryDiagnoser]
public class MyBenchmarks2
{
    [Benchmark]
    public void CalculateAverageAgeWithSpan()
    {
        Performance.CalculateAverageAgeWithSpan();
    }
    [Benchmark]
    public void CalculateAverageAge()
    {
        Performance.CalculateAverageAge();
    }
}
