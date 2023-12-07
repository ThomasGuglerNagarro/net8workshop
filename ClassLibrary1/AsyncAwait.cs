using System.Diagnostics;

namespace ClassLibrary1;


public class AsyncAwait
{
    public async Task<string> RunSomethingGood()
    {
        var repo = new Repository();
        await repo.GetSomethingAsync().ConfigureAwait(false);
        // do heavy calculation => no context switch to calling thread! (maybe UI thread!)
        // repo.DoSomethingHeavy();
        return await repo.GetSomethingAsync().ConfigureAwait(false);
    }

    public void RunSomethingBad()
    {
        var repo = new Repository();
        repo.BadBackgroundAsync();
        // Task.Run(repo.GoodBackgroundAsync); // wraps exception Task.Run=Fire and forget, Service würde zuumindest weiterlaufen! TODO: ein exception handler fehlt dennoch, und erzeugt immer noch neue threads..
    }
}

public class Repository
{
    public async Task<string> GetSomethingAsync()
    {
        await Task.Delay(500);
        return "sdfsdfsdf"; //  await Task.FromResult("Hello world!");
    }

    public async Task<int> GetIntAsync()
    {
        return await Task.FromResult(1);
    }

    public async void BadBackgroundAsync() // Bad: Async void
    {
        var result = await this.GetIntAsync();
        await Task.Delay(5000);
        throw new Exception("bad..");
        /*
        try
          {
              await Task.Delay(5000);
              throw new Exception("bad..");
          }
          catch (Exception ex)
          {
              // log
              throw; // no Task.innerexception..
          }*/
        Console.WriteLine($"The number is {result}");
    }

    public async Task GoodBackgroundAsync() // Besser: async Task
    {
        var result = await this.GetIntAsync();
        await Task.Delay(5000);
        throw new Exception("bad..");
        Console.WriteLine($"The number is {result}");
    }

    public async Task<int> GetValueBadAsync(int number)
    {
        return await Task.Run(() => { return number * 2; });  // waste threadpoool thread
    }

    public async Task<int> GetValueGoodAsync(int number)
    {
        return await Task.FromResult(number * 2); // kein thread vom threadpool
    }

    public async ValueTask<int> GetValueBestAsync(int number) // nicht am heap, sondern nur mehr am stack
    {
        return await new ValueTask<int>(number * 2);
    }
}


public static class PreferAwaitOverContinueWith
{
    internal static void DemoBad()
    {
        var service = new SomeService();
        var number = service.GetIntAsync().ContinueWith(task => task.Result + 2);
    }
    internal static async Task<int> DemoGood()
    {
        var service = new SomeService();
        var number = await service.GetIntAsync();
        number += 2;
        return number;
    }

    internal class SomeService
    {
        internal async Task<int> GetIntAsync()
        {
            return await Task.FromResult(1);
        }
    }
}

public static class AlwaysPassCancellationToken
{
    internal static async Task<int> Demo()
    {
        var svc = new SomeService();
        var source = new CancellationTokenSource();
        Task.Run(async () =>
        {
            await Task.Delay(2000);
            source.Cancel();
        });
        var number = await svc.GetIntAsync(source.Token);
        return number;
    }
    // Pass token , special with IO calls (http,network, database, files), when requests are cancelled:
    internal class SomeService
    {
        internal async Task<int> GetIntAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(5000, cancellationToken);
                //manual: cancellationToken.ThrowIfCancellationRequested();
                return await Task.FromResult(1); // DB Calls..get called even if call is cancelled..
            }
            catch (TaskCanceledException ex)
            {
                // throw; Silent catch TaskCanceledException can be ok in that case
                return await Task.FromResult(0);
            }
        }
    }
}

public static class PreferAsyncTaskOverTask
{
    /// <summary>
    /// https://www.youtube.com/watch?v=Q2zDatDVnO0
    /// https://sharplab.io/
    /// </summary>
    internal class SomeService
    {
        public Task<int> GetValueBadAsync()
        {
            return Task.FromResult(1); // some io call, problem: Exception wrapped in Task!, 
        }

        public async Task<int> GetValueGoodAsync()
        {
            return await Task.FromResult(1); // use state machine
        }
    }
}


public class FakeController
{
    public async Task<int> GetSomething1()
    {
        await Task.Delay(1000);
        return await Task.FromResult(1);
    }
    public async Task<int> GetSomething2()
    {
        await Task.Delay(1000);
        return await Task.FromResult(2);
    }
    public async Task<int> GetSomething2WithException()
    {
        await Task.Delay(1000);
        return await Task.FromResult(2);
    }
    public async Task<int> GetSomething3()
    {
        await Task.Delay(1000);
        return await Task.FromResult(3);
    }
    public async Task<int> GetSomething3WithException()
    {
        await Task.Delay(1000);
        return await Task.FromResult(3);
    }
}

public class RunFasterWithTaskWhenAll()
{
    public async Task DemoSlow() // sequentiell
    {
        var controller = new FakeController();
        var sw = Stopwatch.StartNew();
        await controller.GetSomething1();
        await controller.GetSomething2();
        await controller.GetSomething3();
        Console.WriteLine($"Elapsed {sw.ElapsedMilliseconds}ms.");
    }
    public async Task DemoFast() // parallel
    {
        var controller = new FakeController();
        var sw = Stopwatch.StartNew();
        var task1 = controller.GetSomething1();
        var task2 = controller.GetSomething2();
        var task3 = controller.GetSomething3();
        await Task.WhenAll(task1, task2, task3);
        var result = task1.Result; // hier ist result OK, da der task schon computed ist, somit kein problem!
        Console.WriteLine($"Elapsed {sw.ElapsedMilliseconds}ms.");
    }
    public async Task DemoFastBadWithException()
    {
        var controller = new FakeController();
        var sw = Stopwatch.StartNew();
        var taskCompletionSource = new TaskCompletionSource<int>();
        taskCompletionSource.TrySetException(new Exception[]
        {
            new("error1"),
            new("error2"), // no one gets that!
        });
        var result = await TaskExt.WhenAll(taskCompletionSource.Task);
    }
    public async Task DemoFastGoodWithException()
    {
        var controller = new FakeController();
        var sw = Stopwatch.StartNew();
        var taskCompletionSource = new TaskCompletionSource<int>();
        taskCompletionSource.TrySetException(new Exception[]
        {
            new("error1"),
            new("error2"),
        });
        var result = await TaskExt.WhenAll(taskCompletionSource.Task);
    }
}

public class TaskExt
{
    public static async Task<IEnumerable<T>> WhenAll<T>(params Task<T>[] tasks)
    {
        var allTasks = Task.WhenAll(tasks);
        try
        {
            return await allTasks;
        }
        catch (Exception)
        {
            // ignore
        }
        throw allTasks.Exception ?? throw new Exception("never happen");
    }
}
