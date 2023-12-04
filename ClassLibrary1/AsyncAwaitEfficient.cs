using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ClassLibrary1;

public static class AwaitalbeInt
{
    public static TaskAwaiter GetAwaiter(this int milliseconds)
    {
        return Task.Delay(TimeSpan.FromMilliseconds(milliseconds)).GetAwaiter();
    }
}
/// <summary>
/// https://www.youtube.com/watch?v=n6kiJKr4_oA
/// </summary>
internal class AsyncAwaitEfficient
{
    internal async Task SomeFun()
    {
        await 2000;
    }
    internal void OldSchoolRunAndUpdateUIWithTPL()
    {
        var heavyTask = Task.Run(() => Thread.Sleep(2000)); // dont!
        var c = heavyTask.ContinueWith(_ =>
        {
            var result = 1 + 3;
            // Update UI Thread Dispatcher.Invoke(()=> label.Text = result);
        });
        // Check error: c.ContinueWith(c => c.IsFaulted)
    }

    /// <summary>
    /// hide complexity
    /// more readability
    /// </summary>
    internal async Task AsyncAndAwaitAreJustContextualKeywords()
    {
        // asp.net! => uses iis threadpool
        var heavyTask = Task.Run(() => Thread.Sleep(2000));
        var c = heavyTask.ContinueWith(_ =>
        {
            var result = 1 + 3;
            return result;
        });
        var result = await c; // .ConfigureAwait(false); ACHTUNG: kommt im thread des inneren threads zurück, war bei asp.net core anfangs nötig, nun nicht mehr!
        Console.WriteLine(result);
    }
    internal async void AsyncVoidIsBad()
    {
        var x = 10;
        await Task.Delay(1000);
        throw new Exception();
        // try { AsyncVoidIsBad(); } does not handly anything, just task creation!
    }
    internal async Task AsyncTaskWorksBetter()
    {
        var x = 10;
        await Task.Delay(1000);
        throw new Exception();
        // try { await AsyncTaskWorksBetter(); } => works!
    }
    internal void DeadlockDemo()
    {
        DeadlockImplementation().Wait();
    }
    private async Task DeadlockImplementation()
    {
        // UIText += "12123123123";
        await Task.Delay(2000);
        // UIText += "12123123123";
    }
}

/// <summary>
/// https://www.youtube.com/watch?v=J0mcYVxJEl0
/// https://codetraveler.io/NDCOslo-AsyncAwait
/// </summary>
internal class AsyncAwaitCommonMistakes
{
    public AsyncAwaitCommonMistakes()
    {
        try
        {
            RefreshBad();
        }
        catch (Exception ex)
        {
            Console.WriteLine("will never fail");
        }
        // weiterer code könnte hier zur einer race condition führen
    } 
    async void RefreshBad()
    {
        await ExecuteRefreshCommandBad();
    }
    internal async Task ExecuteRefreshCommandBad()
    {
        // SetIsRefreshing(true).Wait(); // wait circle in UI

        try
        {
            var data = await GetDataBad();
        }
        finally
        {
            //     SetIsRefreshing(false).Wait();
        }
    }
    internal async Task ExecuteRefreshCommandGood()
    {
        // await SetIsRefreshing(true);
        try
        {
            var data = await GetDataBad();
        }
        finally
        {
            //  await SetIsRefreshing(false);
        }
    }
    async Task<List<string>> GetDataBad()
    {
        var data = new List<string>();
        var dataIds = await GetDataIdsBad();
        for (var i = 0; i < dataIds.Count; i++)
        {
            data.Add(await GetDataItemBad(i));
        }
        return data;
    }
    async Task<List<string>> GetDataGood()
    {
        var data = new List<string>();
        var dataIds = await GetDataIdsGood().ConfigureAwait(false); // continue with same thread
        for (var i = 0; i < dataIds.Count; i++)
        {
            data.Add(await GetDataItemGood(i).ConfigureAwait(false));
        }
        return data;
    }
    async Task<List<string>> GetDataIdsBad()
    {
        /*
        if (Cache.Contains())
            return Cache; */
        try
        {
            return await Task.FromResult(new List<string>());
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }
    /// <summary>
    /// https://www.youtube.com/watch?v=mEhkelf0K6g
    /// When to use ValueTask instead of Task and save precious memory in C#
    /// </summary>
    /// <remarks>
    /// NOT: mehrfach await, parallel, getawaiter().getresult =>
    /// 
    /// </remarks>
    /// <returns></returns>
    async ValueTask<List<string>> GetDataIdsGood()
    {
        /* => BESSER: ValueTask! nicht am heap, sondern am stack: kein GC, memory allocation besser => HOTpath verwendet nicht await , dann valuetask
      if (Cache.Contains())
          return Cache; */
        try
        {
            return await Task.FromResult(new List<string>()); // await muss bleiben! sonst keine execution
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }
    async Task<string> GetDataItemBad(int index)
    {
        return await GetDataItemFromAPI(index);
    }
    Task<string> GetDataItemGood(int index) // kein async nötig bei nur einem callm, kein context switch nötig
    {
        return GetDataItemFromAPI(index);
    }
    Task<string> GetDataItemFromAPI(int index)
    {
        return Task.FromResult("Line");
    }
}
