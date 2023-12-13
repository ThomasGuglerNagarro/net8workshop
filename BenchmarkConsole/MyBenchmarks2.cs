using BenchmarkDotNet.Attributes;
using ClassLibrary1;

namespace BenchmarkConsole;
/*
 Method                      | Mean      | Error    | StdDev   | Gen0   | Allocated |
|---------------------------- |----------:|---------:|---------:|-------:|----------:|
| CalculateAverageAgeWithSpan |  99.07 ns | 1.160 ns | 1.028 ns | 0.0408 |     256 B |
| CalculateAverageAge         | 180.07 ns | 0.903 ns | 0.754 ns | 0.0994 |     624 B |
 */
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
