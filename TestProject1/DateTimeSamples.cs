namespace TestProject1;

internal class DateTimeSamples(TimeProvider p)
{
    public DateTimeOffset GetTime() => p.GetUtcNow();

    public async Task DoPeriodic()
    {
        var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1), p);
        while (await periodicTimer.WaitForNextTickAsync())
        {
            Console.WriteLine(GetTime());
        }

        await Task.Delay(TimeSpan.FromDays(1), p);

        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1), p);
    }
}
