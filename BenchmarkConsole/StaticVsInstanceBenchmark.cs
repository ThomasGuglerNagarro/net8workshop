using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Mathematics;

namespace BenchmarkConsole;

/* .NET 8.0.18 (8.0.1825.31117),
   | Method                               | Mean        | Error     | StdDev    | Gen0   | Allocated |
   |------------------------------------- |------------:|----------:|----------:|-------:|----------:|
   | StaticMethod                         |    688.3 ns |   8.02 ns |   6.69 ns |      - |         - |
   | InstanceMethod                       |    756.7 ns |   8.66 ns |   8.10 ns |      - |         - |
   | InstanceMethodWithAllocation         |  2,468.0 ns |  49.25 ns |  67.41 ns | 3.8223 |   24000 B |
   | StaticMethodWithComplexCalculation   | 17,528.8 ns | 245.22 ns | 229.38 ns |      - |         - |
   | InstanceMethodWithComplexCalculation | 21,679.6 ns | 420.59 ns | 372.84 ns |      - |         - |
----------------------
.NET 9.0.7 (9.0.725.31616),
   | Method                               | Mean        | Error     | StdDev   | Gen0   | Allocated |
   |------------------------------------- |------------:|----------:|---------:|-------:|----------:|
   | StaticMethod                         |    582.8 ns |   7.81 ns |  7.31 ns |      - |         - |
   | InstanceMethod                       |    604.6 ns |   5.84 ns |  5.47 ns |      - |         - |
   | InstanceMethodWithAllocation         |  2,347.1 ns |  46.96 ns | 85.87 ns | 3.8223 |   24000 B |
   | StaticMethodWithComplexCalculation   | 16,644.3 ns | 104.02 ns | 92.21 ns |      - |         - |
   | InstanceMethodWithComplexCalculation | 20,922.1 ns |  67.51 ns | 56.38 ns |      - |         - |
 */
[MemoryDiagnoser]
[SimpleJob]
public class StaticVsInstanceBenchmark
{
    private readonly MathHelper _mathHelper = new MathHelper();
    private readonly int[] _numbers = Enumerable.Range(1, 1000).ToArray();

    [Benchmark]
    public int StaticMethod()
    {
        int sum = 0;
        for (int i = 0; i < _numbers.Length; i++)
        {
            sum += MathHelper.CalculateSquareStatic(_numbers[i]);
        }
        return sum;
    }

    [Benchmark]
    public int InstanceMethod()
    {
        int sum = 0;
        for (int i = 0; i < _numbers.Length; i++)
        {
            sum += _mathHelper.CalculateSquareInstance(_numbers[i]);
        }
        return sum;
    }

    [Benchmark]
    public int InstanceMethodWithAllocation()
    {
        int sum = 0;
        for (int i = 0; i < _numbers.Length; i++)
        {
            // Neue Instanz bei jedem Aufruf - zeigt den Overhead
            var helper = new MathHelper();
            sum += helper.CalculateSquareInstance(_numbers[i]);
        }
        return sum;
    }

    [Benchmark]
    public int StaticMethodWithComplexCalculation()
    {
        int sum = 0;
        for (int i = 0; i < _numbers.Length; i++)
        {
            sum += MathHelper.ComplexCalculationStatic(_numbers[i]);
        }
        return sum;
    }

    [Benchmark]
    public int InstanceMethodWithComplexCalculation()
    {
        int sum = 0;
        for (int i = 0; i < _numbers.Length; i++)
        {
            sum += _mathHelper.ComplexCalculationInstance(_numbers[i]);
        }
        return sum;
    }
}
public class MathHelper
{
    private readonly int _multiplier = 2;

    // Static method - kein Instanz-Overhead
    public static int CalculateSquareStatic(int number)
    {
        return number * number;
    }

    // Instance method - benötigt 'this' pointer
    public int CalculateSquareInstance(int number)
    {
        return number * number;
    }

    // Static method mit komplexerer Berechnung
    public static int ComplexCalculationStatic(int number)
    {
        int result = number;
        for (int i = 0; i < 10; i++)
        {
            result = (result * 2 + 1) % 1000;
        }
        return result;
    }

    // Instance method mit komplexerer Berechnung und Verwendung von Instanz-Feld
    public int ComplexCalculationInstance(int number)
    {
        int result = number;
        for (int i = 0; i < 10; i++)
        {
            result = (result * _multiplier + 1) % 1000;
        }
        return result;
    }
}
