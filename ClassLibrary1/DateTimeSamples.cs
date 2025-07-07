namespace ClassLibrary1;

internal class DateTimeSamples(TimeProvider p)
{
    public DateTimeOffset GetTime() => p.GetUtcNow();

    public async Task DoPeriodicNOTworking()
    {
        /*
        var periodicTimer = new System.Threading.PeriodicTimer(TimeSpan.FromSeconds(1), p);
        while (await periodicTimer.WaitForNextTickAsync())
        {
            Console.WriteLine(GetTime());
        }
        await Task.Delay(TimeSpan.FromDays(1), p);
        */
    }
}
