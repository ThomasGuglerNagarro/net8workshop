namespace ClassLibrary1;

public class NullableReferenceTypeDemo2
{
    public NullableReferenceTypeDemo2? MyProperty { get; set; }

    public static void Demo(NullableReferenceTypeDemo2? input)
    {
        Console.WriteLine(input.MyProperty);
    }
}
public static class NullableReferenceTypeDemo
{
    public static void Demo()
    {
        var p2 = new PersonSampleGood();
        p2.Details = new() { FirstName = "Test", LastName = "Test2" };
        Console.WriteLine(p2.Details); // TODO : why is this not null?
        // p2.Details.FirstName = "Test";
        // IS working:
        // typeof(DetailsGood).GetProperty(nameof(DetailsGood.FirstName))?.SetValue(p2.Details, "bla");
        typeof(DetailsGood).GetProperty(nameof(DetailsGood.MiddleName))?.SetValue(p2.Details, "asdsdf");

        var p = new PersonSampleBad();
        // p.Details = new() { FirstName = "Test", LastName = "Test2" };
        if (p.Details?.LastName is not null)
        {
            Console.WriteLine(p.Details.LastName.Length);
        }
        // ArgumentNullException.ThrowIfNull(p.Details);
        // ArgumentNullException.ThrowIfNull(p.Details.LastName);
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
    public DetailsGood? MyProperty { get; set; }
    public required string FirstName { get; init; }
    public string MiddleName { get; private set; } // not a good idea...
    public string? LastName { get; init; }
}
