namespace ClassLibrary1;

public static class NullableReferenceTypeDemo
{
    public static void Demo()
    {
        var p2 = new PersonSampleGood();
        p2.Details = new() { FirstName = "Test", LastName = "Test2" };
        // NOT working p2.Details.FirstName = "Test";
        // IS working:
        typeof(DetailsGood).GetProperty(nameof(DetailsGood.FirstName))?.SetValue(p2.Details, "bla");

        var p = new PersonSampleBad();
        // 1 p.Details = new() { FirstName = "Test", LastName = "Test2" };
        // 2 if (p.Details?.LastName != null)
        // 3 ArgumentNullException.ThrowIfNull(p.Details);
        // 3 ArgumentNullException.ThrowIfNull(p.Details.LastName);
        Console.WriteLine(p.Details.LastName.Length);
    }
    internal static IEnumerable<PersonSampleBad> Demo2()
    {
        // BAD return null!;
        yield return new() { Details = new() { FirstName = "" } };
    }
}
internal class PersonSampleBad
{
    public DetailsBad Details { get; set; }
}

internal class DetailsBad
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

internal class PersonSampleGood
{
    public DetailsGood? Details { get; set; }
}


public class DetailsGood
{
    public required string FirstName { get; init; }
    public string? LastName { get; init; }
}