// alias any type 
using Personn = (string FirstName, string LastName);
using ints = int[];
using System.Collections.Immutable;
using Grade = decimal; // System.Decimal
using MyPoint = (int x, int y);
using Grade2 = (decimal grade, decimal weight);
using unsafe Grade3 = (decimal grade, decimal weight)*;

namespace ClassLibrary1;

// Microsoft.Bcl.TimeProvider for TimeProvider
public class PaydayCalculator(TimeProvider timeprovider)
{
    public PaydayCalculator() : this(TimeProvider.System)
    {
    }
    public bool IsPayday()
    {
        var today = DateOnly.FromDateTime(timeprovider.GetUtcNow().DateTime);
        var thisMonthsPayday = new DateOnly(today.Year, today.Month, 1).AddMonths(-1).AddDays(-1);
        thisMonthsPayday = thisMonthsPayday.DayOfWeek switch
        {
            DayOfWeek.Saturday => thisMonthsPayday.AddDays(-1),
            DayOfWeek.Sunday => thisMonthsPayday.AddDays(-2),
            _ => thisMonthsPayday
        };
        return thisMonthsPayday == today;
    }
}

// inlinearray BCL => .net standard missing
// interceptors ..
public class NET8
{
    public static void NewFeatures()
    {
        // new terse syntax : the goal is to provide a unified and user-friendly API
        // for declaring collections.
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

    public static void NewCollectionFeatures()
    {
        List<int> listOfNumbers = new() { 1, 2, 3 };
        List<int> listOfNumbersNew = [1, 2, 3];

        int[] arrayOfNumbers = { 1, 2, 3 };
        int[] arrayOfNumbersNew = [1, 2, 3];

        Span<int> spanOfNumbers = stackalloc int[] { 1, 2, 3 };
        Span<int> spanOfNumbersNew = [1, 2, 3];
        Dictionary<int, string> dictionary = new()
        {
            {1,"bla" },
            {2,"bla2" },
        };
        Dictionary<int, string> dictionaryNew = [];

        List<int> allNumbers = [.. listOfNumbersNew, 4, .. spanOfNumbers];

        ImmutableArray<int> ints = ImmutableArray.Create(arrayOfNumbers);
    }

    public static void DemoPrimaryCtorClasses()
    {
        var mads = new Student("Mads Torgersen", 900751, new[] { 3.5m, 2.9m, 1.8m });
        System.Console.WriteLine(mads.GPA);
        System.Console.WriteLine(mads);
    }
    // Collection Expression
    public static void DemoCollectionExpressions()
    {
        var others = new[] { 1.0m, 2.0m, 3.0m };
        var mads = new Student("Mads Torgersen", 900751, [3.5m, 2.9m, 1.8m, .. others]);
    }

    public class Student(string name, int id, Grade[] grades)  // primary ctor bei klassen => name+id nicht mehr public props
    {
        public string Name { get; } = name; // auto property; 1x zugewisen
                                            // zukunft: set => field = value.Trim(); 
        public int Id => id; // computed property, bei jedem access IMMER ausgelesen!
        public Student(string name, int id) : this(name, id, Array.Empty<Grade>()) { }
        public Grade GPA => grades switch // switch expression
        {
            [] => 4.0m,
            [var grade] => grade,
            [.. var all] => all.Average()
        };
    }
}
