using BenchmarkDotNet.Attributes;
using ClassLibrary1;

namespace BenchmarkConsole;

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
