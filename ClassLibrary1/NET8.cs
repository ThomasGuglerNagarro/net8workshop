namespace ClassLibrary1;

// alias any type 
using Personn = (string FirstName, string LastName);
using ints = int[];

// inlinearray BCL => .net standard missing
// interceptors
public class NET8
{
    public static void NewFeatures()
    {
        // new terse syntax : the goal is to provide a unified and user-friendly API for declaring collections.
        List<int> start = [1, 2, 3];
        int[] start2 = { 1, 2, 3 };
        Span<int> end = [5, 6, 7];
        List<int> alle = [.. start, 4, .. end]; // Anmekrung: CollectionBuilder attrib für dieses Feature bei eigenen klassen: nicht möglich bei .net standard
        // List<int> end = { 5, 6, 7 }; // not working
        Span<int> diceNumbers = [1, 2, 3, 4, 5, 6];
        // Random.Shared.Shuffle(diceNumbers); => not working .net standard

        List<int> list = [1, 2, 3, 4];
        // var frozenSet = list.ToFrozenSet(); => => not working .net standard

        // Primary constructors eliminate the need for declaring private fields and manually linking parameter values to those fields in constructor bodies. Say goodbye to that tedious process:
        var p = new Person("Seth", "Gecko");
        // p.FirstName = "Alan"; ERROR record are immutable by default

        Console.WriteLine(nameof(p.LastName)); // nameof now with members names

    }

    public record Person(string FirstName, string LastName); // generate public read-only properties. 
    public class Person3(string FirstName, string LastName); // generate private fields. 


}
