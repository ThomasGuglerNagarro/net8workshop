namespace ClassLibrary1;
// Asynchronous streams

public class AsyncIterators
{
    public static async Task Demo()
    {
        await foreach (var number in GenerateSequence())
        {
            Console.WriteLine($"At Time {DateTime.Now:hh:mm:ss} retreived {number}");
            // cts.Token.ThrowIfCancellationRequested();
            // if (cts.Token.IsCancellationRequested) break;
        }
    }

    internal static async IAsyncEnumerable<int> GenerateSequence() // IAsyncEnumerable
    {
        for (int i = 0; i < 50; i++)
        {
            // every 10 elements wait 2 seconds:
            if (i % 10 == 0)
                await Task.Delay(2000);
            yield return i;
        }
    }

    // using scope; pattern matching, async stream
    private async IAsyncEnumerable<string> GetLineByLineAsync()
    {
        using var stream = new StreamReader(@"C:\bla\bla..");
        while (await stream.ReadLineAsync() is string line)
        {
            await Task.Delay(100);
            yield return line;
        }
        // implicit Dispose
    }
}
