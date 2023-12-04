using Microsoft.Extensions.Caching.Memory;
using System.Buffers;
using System.Collections.Concurrent;
using System.Globalization;
using System.Numerics;

namespace ClassLibrary1;

public class Performance
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private readonly object _syncRoot = new object();
    private readonly List<int> _list = new List<int>();

    public void DemoStackallocAndSpanBegin()
    {
        var a1 = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 0 }; // reference type    
        var a2 = a1[2..6]; // 2,3,4,5 + Copy of a1
        var a3 = a1.AsSpan()[2..6]; // 2,3,4,5 + NO Copy of a1
        // var a3 = a1.AsSpan().Slice(2, 4); 
        a3[0]= 500;
        Console.WriteLine(a1[0]); // ?
        Console.WriteLine(a2[0]);
        Console.WriteLine(a3[0]);
    }

    public async Task ConcurrentSample()
    {
        /*
        var dataList = new List<string>();
        using (var reader = File.OpenText(@"C:\Users\thomasgugler\source\repos\ConsoleApp4\Samples\Performance.cs"))
        {
            var fileText = await reader.ReadToEndAsync();
            var data = fileText.Split('\n');
            dataList = new List<string>(data);
             //while (!reader.EndOfStream)
             //{
             //    var fileText = await reader.ReadLineAsync();
             //    dataList.Add(fileText);
             //}
        }*/

        var data = File.ReadAllLines(@"C:\Users\thomasgugler\source\repos\ConsoleApp4\Samples\Performance.cs");
        var dataList = new List<string>(data);
        await ProcessManyItems(dataList); // ProcessManyItemsBetter
    }

    public async Task ParallelWithPartitionerSample()
    {
        // efficiently distribute workloads into chunks
        var data = Enumerable.Range(0, 100);
        var partitioner = Partitioner.Create(data);
        Parallel.ForEach(partitioner, item => PerformExpensiveOperation(item));
    }

    public async Task InMemoryCacheSample()
    {
        var data = Enumerable.Repeat(10, 5);
        foreach (var i in data)
        {
            var result = await GetItemById(i);
            Console.WriteLine(result);
        }
        async Task<string> GetItemById(int i)
        {
            if (!_cache.TryGetValue(i, out string? value))
            {
                Console.WriteLine("no cache hit..read from db...");
                await Task.Delay(1000);
                _cache.Set(i, i.ToString());
            }
            return value;
        }
    }

    public async Task LockedDataStrucuture()
    {
        var data = Enumerable.Range(0, 10);
        // BAD
        // Parallel.ForEach(data, WithLock);
        // GOOD: ConcurrentBag, ConcurrentQueue, or ConcurrentDictionary
        var _bag = new ConcurrentBag<int>(); // private readonly 
        // Parallel.ForEach(data, WithConcurrentBag);
        // BETTER: SemaphoreSlim:
        var _semaphoreSlim = new SemaphoreSlim(1 , 1); // private readonly 
        var _list = new List<int>(); // private readonly 
        Parallel.ForEach(data, async i => await WithSemaphoreSlim(i));
        // await Parallel.ForEachAsync()
        void WithLock(int i)
        {
            lock (_syncRoot)
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                _list.Add(i);
            }
        }
        async Task WithSemaphoreSlim(int item)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                _list.Add(item);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        void WithConcurrentBag(int i)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            _bag.Add(i);
        }

    }

    public async Task StackallocCalculateSum()
    {
        Console.WriteLine(CalculateSumBetter(4));
        double CalculateSum(double[] values)
        {
            double sum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }
            return sum;
        }
        double CalculateSumBetter(int count)
        {
            double sum = 0;
            Span<double> values = stackalloc double[] {0, 1,2,3}; // Allocate memory on the stack
            for (int i = 0; i < count; i++)
            {
                sum += values[i];
            }
            return sum;
        }
    }

    public async Task HashSet()
    {
        // unordered collection of unique elements. 
        var userList = new HashSet<int>() { 10, 100, 100, 1000 };
        await Task.CompletedTask;
    }

    public async Task SIMDsupport()
    {
        var data = new float[] {0, 1, 2, 3, 4 };
        Normalize(data);
        await Task.CompletedTask;
    }


    private void Normalize(float[] data)
    {
        if (Vector.IsHardwareAccelerated) // Check for SIMD support
        {
            Vector<float> factor = new Vector<float>(0.5f);
            for (int i = 0; i < data.Length; i += Vector<float>.Count)
            {
                Vector<float> vector = new Vector<float>(data, i);
                (vector * factor).CopyTo(data, i);
            }
        }
        else
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] /= 2f;
            }
        }
    }

    private void PerformExpensiveOperation(int item)
    {
        Console.WriteLine(item);
    }

    private async Task ProcessManyItems(List<string> items)
    {
        var tasks = items.Select(async item => await ProcessItem(item));
        await Task.WhenAll(tasks);
    }

    private async Task ProcessManyItemsBetter(List<string> items, int maxConcurrency = 10)
    {
        using var semaphore = new SemaphoreSlim(maxConcurrency);
        var tasks = items.Select(async item =>
        {
            await semaphore.WaitAsync(); // Limit concurrency by waiting for the semaphore.
            try
            {
                await ProcessItem(item).ConfigureAwait(false);
            }
            finally
            {
                semaphore.Release(); // Release the semaphore to allow other operations.
            }
        });

        await Task.WhenAll(tasks);
    }

    private async ValueTask<string> ProcessItem(string item)
    {
        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        Console.WriteLine(item);
        await Task.Delay(500).ConfigureAwait(false);
        return await Task.FromResult(item).ConfigureAwait(false);
    }

    public static void DateWithStringAndSubstringDemo()
    {
        string dateAsText = "02 07 2021";
        var date = DateWithStringAndSubstring(dateAsText);
        Console.WriteLine(date);
    }

    internal static (int day, int month, int year) DateWithStringAndSubstring(string inputString)
    {
        // Problem: new string on heap + GC(!)
        var dayAsText = inputString.Substring(0, 1);
        var monthAsText = inputString.Substring(3, 2);
        var yearAsText = inputString.Substring(6);
        var day = int.Parse(dayAsText);
        var month = int.Parse(monthAsText);
        var year = int.Parse(yearAsText);
        return (day, month, year);
    }
    /* not with .net standard 2.0: int.Parse in System.Runtime
    internal static (int day, int month, int year) DateWithStringAndSpan(string inputString)
    {
        // no alloc!, no GC!, ist immer am stack, ist nur eine liste von adressen(mit Offset) am stack auf den heap.
        ReadOnlySpan<char> dateAsSpan = inputString; //.AsSpan();
        var dayAsText = dateAsSpan.Slice(0, 1);
        var monthAsText = dateAsSpan.Slice(3, 2);
        var yearAsText = dateAsSpan.Slice(6);
        var day = int.Parse(dayAsText);
        var month = int.Parse(monthAsText);
        var year = int.Parse(yearAsText);
        return (day, month, year);
    } */
}

// Span<T> as part of System.Runtime.dll is not available in .NET Standard 2.0.
// There's a NuGet package (System.Memory), but that provides only those types and not the changes to the BCL to use Span<T>.

internal class Span2
{
    internal void ArraySegmentDemo()
    {
        var data = new ArraySegment<byte>(Guid.NewGuid().ToByteArray());
        var guidBuffer = new byte[16];
        Buffer.BlockCopy(data.Array, data.Offset, guidBuffer, 0, 16);
        var lockTokenGuid = new Guid(guidBuffer);
        Console.Write(lockTokenGuid.ToString());
    }
    internal void DemoNoAllocationBut2xSlower()
    {
        var data = new ArraySegment<byte>(Guid.NewGuid().ToByteArray());

        byte[] guidBuffer = ArrayPool<byte>.Shared.Rent(16);

        Buffer.BlockCopy(data.Array, data.Offset, guidBuffer, 0, 16); // supports endian
        var lockTokenGuid = new Guid(guidBuffer);

        ArrayPool<byte>.Shared.Return(guidBuffer);

        Console.Write(lockTokenGuid.ToString());
    }

    internal void DemoStackalloc()
    {
        // not safe code (byte little endian => copy to could fail)
        var data = new ArraySegment<byte>(Guid.NewGuid().ToByteArray());

        Span<byte> guidBytes = stackalloc byte[16];
        data.AsSpan().CopyTo(guidBytes);
        var lockTokenGuid = new Guid(guidBytes.ToArray()); // .NET standard: .ToArray()
        Console.Write(lockTokenGuid.ToString());
    }

    internal void DemoSpanOfInt() // Span= am Stack ein array von pointern auf den heap
    {
        int[] data = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 0 };
        // var dataSpan = data.AsSpan()[2..6]; // geht im implizit duch Span<int> dataSpan = data;
        // Span<int> dataSpan = data;
        Span<int> dataSpan = data.AsSpan().Slice(2, 4);
        dataSpan[0] = 500;
        foreach (var i in data) Console.WriteLine(i);
        Console.WriteLine();
        foreach (var i in dataSpan) Console.WriteLine(i);
    }

    internal void DemoSpanOfStrings()
    {
        var strSpan = "bla".AsSpan(); // => ReadOnlySpan, weil string immutable
        foreach (var i in strSpan) Console.WriteLine(i);
        // zb. list etc geht NICHT, weil nicht immutable
    }

    internal void DemoStackallocUnsafeOld()
    {
        // Allowunsafe ! => ALTE Variante
        unsafe
        {
            int* data = stackalloc int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 0 };
        }
    }

    internal void DemoStackallocNewWithSpan()
    {
        Span<int> data = stackalloc int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 0 };
        foreach (var i in data) Console.WriteLine(i);
    }
    internal void DemoPoint3D()
    {
        // var p = new Point3D(1.1, 2.2, 3.3);
        // var p = Point3D.ParseWithMemoryProblem("(1.1, 2.2, 3.3)");
        var p = Point3D.ParseBetter("(1.1, 2.2, 3.3)");
        Console.WriteLine(p);
    }
}

public record struct Point3D(double X, double Y, double Z)
{
    private const int MaxStackAllocSize = 256;

    internal static Point3D ParseWithMemoryProblem(string input) // allocates 248 byte => gc!
    {
        try
        {
            // klammern entfernen, 3 Teile genrieren
            var items = input.Replace("(", "").Replace(")", "").Split(',');
            return new Point3D(
                double.Parse(items[0], CultureInfo.CurrentUICulture),
                double.Parse(items[1], CultureInfo.CurrentUICulture),
                double.Parse(items[2], CultureInfo.CurrentUICulture));
        }
        catch (Exception e)
        {
            throw new FormatException("input incorrect format", e);
        }
    }
    internal static Point3D ParseBetter(string input) // without any heap memory allocation, und etwas schneller
    {
        try
        {
            // input in span umwandeln und initialisieren
            var chars = input.AsSpan();
            Span<double> coords = stackalloc double[] { 0.0, 0.0, 0.0 }; // initialize with 0! nicht sicher bei stackalloc!
            Span<char> number = chars.Length < MaxStackAllocSize ? stackalloc char[chars.Length] : new char[chars.Length]; // maximale länger muss gesetzt werden..müsste auf max. länge geprüft werden!
            number.Fill(' '); // init mit spaces
            // work:
            int count = 0; int pos = 0;
            foreach (var c in chars)
            {
                if (c == '(') // Start
                    continue;
                // ende?
                if (c == ',' || c == ')')
                {
                    // double.Parse(number, CultureInfo.CurrentUICulture); = keine heap alloc weil inpiut = span
                    // double.Parse(number.ToString(), CultureInfo.CurrentUICulture); = leider schon heap alloc weil inpiut != span
                    coords[count++] = double.Parse(number.ToString(), CultureInfo.CurrentUICulture);
                    pos = 0;
                    number.Fill(' ');
                    continue; // nächster Teil
                }
                number[pos++] = c;
            }
            return new Point3D(coords[0], coords[1], coords[2]);

        }
        catch (Exception e)
        {
            throw new FormatException("input incorrect format", e);
        }
    }

    public override string ToString() => $"({X},{Y},{Z})";
}