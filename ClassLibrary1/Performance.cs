using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Numerics;

namespace ClassLibrary1;

public class Performance
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private readonly object _syncRoot = new object();
    private readonly List<int> _list = new List<int>();

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
}