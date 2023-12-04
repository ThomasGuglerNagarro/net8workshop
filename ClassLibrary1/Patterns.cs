namespace ClassLibrary1;

internal record PersonRecord(string Name, int Age);

public static class PersonRecordDemo
{
    public static void Demo()
    {
        var p = new PersonRecord("Thomas", 27);
        Console.WriteLine(p.Name);
        var copiedPerson = p with { Age = 46 };
        var switchResult = copiedPerson switch
        {
            ("Thomas", > 2) => "I'm the copy of Thomas",
            ("Thomas", _) => "I'm Thomas",
            _ => throw new ArgumentException()
        };
        Console.WriteLine(switchResult);
    }
}

/*
internal record DateOnly()
{
    public DayOfWeek DayOfWeek { get; }

    internal static DateOnly FromDateTime(DateTime dateTime)
    {
        throw new NotImplementedException();
    }
}*/
internal record Something;
internal record Rectangle(int Height, int Width, Rectangle? InnerRectangle = null) : Something
{
    public void Deconstruct(out int height, out int width)
    {
        height = Height;
        width = Width;
    }
}
internal record Circle : Something;
internal record Car(int Passengers, int Weight);

internal record Bus();
public class Patterns
{
    /// <summary>
    /// Pattern switch expression, DateOnly
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static bool IsWeekDay(DateOnly date) =>
        date.DayOfWeek switch
        {
            DayOfWeek.Saturday => false,
            DayOfWeek.Sunday => false,
            _ => true
        };
    public static string TypePattern()
    {
        Something input = new Rectangle(10, 20);
        // var (height, width) = new Rectangle(10, 20);
        var result = input switch
        {
            Rectangle and { Height: > 5 } rect => $"its a rectangle with height:{rect.Height}",
            Circle => "its a circle else",
            // _ => "its not"
            var x => "its not"
        };
        return result;
    }

    public static bool IsArbeitsPausenZeit(DateTime dateTime) =>
        (IsWeekDay(DateOnly.FromDateTime(dateTime)), dateTime.Hour) switch
        {
            (true, 9) => true,
            (true, >= 12 and <= 13) => true, // relational patterns, "and" logical pattern
            (true, 15) => true,
            (false, _) => false,
            (_, _) => false,
        };

    internal static decimal CalculateToll(object vehicle) => // switch type pattern
        vehicle switch
        {
            Car { Passengers: 0 } _ => 1.00m,
            Car { Passengers: >= 1 and <= 2 } _ => 1.50m,
            Car _ => 2.00m,
            Bus _ => 5.00m,
            { } => throw new ArgumentException("unknown type"),
            null => throw new ArgumentNullException(nameof(vehicle)),
        };

    public static void ListPatterns()
    {
        int[] numbers = { 1, 2, 3 };

        Console.WriteLine(numbers is [1, 2, 3]);  // True
        Console.WriteLine(numbers is [1, 2, 4]);  // False
        Console.WriteLine(numbers is [1, 2, 3, 4]);  // False
        Console.WriteLine(numbers is [0 or 1, <= 2, >= 3]);  // True
        // with slice pattern
        Console.WriteLine(new[] { 1, 2, 3, 4, 5 } is [> 0, > 0, ..]);  // True
        Console.WriteLine(new[] { 1, 1 } is [_, _, ..]);  // True
        Console.WriteLine(new[] { 0, 1, 2, 3, 4 } is [> 0, > 0, ..]);  // False
        Console.WriteLine(new[] { 1 } is [1, 2, ..]);  // False

        Console.WriteLine(new[] { 1, 2, 3, 4 } is [.., > 0, > 0]);  // True
        Console.WriteLine(new[] { 2, 4 } is [.., > 0, 2, 4]);  // False
        Console.WriteLine(new[] { 2, 4 } is [.., 2, 4]);  // True

        Console.WriteLine(new[] { 1, 2, 3, 4 } is [>= 0, .., 2 or 4]);  // True
        Console.WriteLine(new[] { 1, 0, 0, 1 } is [1, 0, .., 0, 1]);  // True
        Console.WriteLine(new[] { 1, 0, 1 } is [1, 0, .., 0, 1]);  // False

        var x = new[] { 1, 2, 3, 4, 5 };
        // old
        if (x.Length > 3 && x[0] == 1 && x[x.Length - 1] == 5)
        {
            Console.WriteLine("Pattern match");
        }
        // new: ".." range operator
        // capture with "var s"
        if (x is [1,_,..var s,5])
        {
            Console.WriteLine(s);
            Console.WriteLine("Pattern match");
        }
    }

    public static void ExtendedPropertyPattern()
    {
        var rectangleInside = new Rectangle(100, 100);
        var rectangle = new Rectangle(200, 300, rectangleInside);
        if (rectangle is { InnerRectangle.Height: > 100 })
        { }
    }

    public static void SwitchPattern()
    {
        string[] data = ["Name1", "Name2", "Name3"];
        var result = data switch
        {
            ["Name1", "Name12", "Name13"] => "all names",
            ["Name1", "Name12", _] => "first two names",
            ["Name1", .. var x] => String.Join(",", x),
            // ["Name1", _, _] => "first name",
            _ => "no name"
        };
        Console.WriteLine(result);
    }

    public static void ListPatterns2()
    {
        var list = new Rectangle[] { new(1, 2), new(3, 4) };
        var result = list switch
        {
            // [Rectangle,..] => "first is rectanble, and dont care about the rest",
            [Rectangle, .. var middle, _] => "first is rectanble,ignore the last, und take the middle elements",
        };
    }

}
