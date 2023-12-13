using BenchmarkDotNet.Attributes;
using ClassLibrary1;

namespace BenchmarkConsole;
/*
 Method    | Mean      | Error    | StdDev   | Gen0   | Allocated |
|---------- |----------:|---------:|---------:|-------:|----------:|
| GetHostV1 | 198.09 ns | 4.028 ns | 7.467 ns | 0.0432 |     272 B |
| GetHostV2 | 418.68 ns | 8.267 ns | 7.329 ns | 0.0138 |      88 B |
| GetHostV3 |  93.65 ns | 1.873 ns | 2.368 ns | 0.0381 |     240 B |
| GetHostV4 |  26.86 ns | 0.363 ns | 0.340 ns | 0.0216 |     136 B |
| GetHostV5 |  57.72 ns | 1.037 ns | 1.487 ns | 0.0139 |      88 B |
 */
[MemoryDiagnoser]
public class MyBenchmarks
{
    private string data = "https://www.orf.at";

    [Benchmark]
    public string GetHostV1() => new Performance().GetHostV1(data);
    [Benchmark]
    public string GetHostV2() => new Performance().GetHostV2(data);
    [Benchmark]
    public string GetHostV3() => new Performance().GetHostV3(data);
    [Benchmark]
    public string GetHostV4() => new Performance().GetHostV4(data);
    [Benchmark]
    public string GetHostV5() => new Performance().GetHostV5(data);
}
