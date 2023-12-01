using System.Collections.Immutable;

namespace ClassLibrary1;

public class Operators
{
    public static void IfElseToBracesToTernaryOperatorToSwitchStatementToSwitchExpression()
    {
        var environment = "PREPROD";
        String configSettings = null;
        if (environment == "UAT")
            configSettings = "UAT";
        else if (environment == "PREPROD")
            configSettings = "PRP";
        else if (environment == "PROD")
            configSettings = "PRD";
        else
            configSettings = string.Empty;
        // 1: Better with braces?
        // 2: better with "?:" ?
        /*
        configSettings = environment == "UAT" ? "UAT" :
            environment == "PREPROD" ? "PRP" :
            environment == "PROD" ? "PRD" : string.Empty; */
        // 3: better with switch statement?
        /*
        switch (environment)
        {
            case "UAT":
                configSettings = "UAT";
                break;
            case "PROPROD":
                configSettings = "PRP";
                break;
            case "PROD":
                configSettings = "PRD";
                break;
            default:
                configSettings = string.Empty;
                break;
        }
        */
        // 4: best? switch expression
        /*
        configSettings = environment switch
        {
            "UAT" => "UAT",
            "PREPROD" => "PRP",
            "PROD" => "PRD",
            _ => string.Empty
        };*/
        // 5: best
        var envMap = new Dictionary<string, string>
        {
            { "UAT", "UAT" },
            { "PREPROD", "PRP" },
            { "PROD", "PRD" }
        }.ToImmutableDictionary();
        // var env = envMap.TryGetValue(environment, out configSettings);
        envMap.GetValueOrDefault(environment, string.Empty);
    }

    public static void Demo()
    {

        var array = new int[] { 1, 2, 3, 4, 5 };
        // index operatur "^" / System.Index
        var thirdItem = array[2];    // array[2]
        var lastItem = array[^1];    // array[new Index(1, fromEnd: true)] == list[list.Count - 1];
                                     // Range operator ".." / System.Range
        var slice1 = array[2..^3];    // array[new Range(2, new Index(3, fromEnd: true))]
        var slice2 = array[..^3];     // array[Range.EndAt(new Index(3, fromEnd: true))]
        var slice3 = array[2..];      // array[Range.StartAt(2)]
        var slice4 = array[..];       // array[Range.All]


        //?? and ??= operators - the null-coalescing operators
        List<int> numbers = null;
        int? a = null;
        Console.WriteLine((numbers is null)); // expected: true
                                              // if numbers is null, initialize it. Then, add 5 to numbers
        (numbers ??= new List<int>()).Add(5);
        Console.WriteLine(string.Join(" ", numbers));  // output: 5
        Console.WriteLine((numbers is null)); // expected: false        
        Console.WriteLine((a is null)); // expected: true
        Console.WriteLine((a ?? 3)); // expected: 3 since a is still null 
                                     // if a is null then assign 0 to a and add a to the list
        numbers.Add(a ??= 0);
        Console.WriteLine((a is null)); // expected: false        
        Console.WriteLine(string.Join(" ", numbers));  // output: 5 0
        Console.WriteLine(a);  // output: 0

        // using var f1 = new FileStream("sdfsdf", FileMode.Open); // using declaration
    }

    public static void MovingAverage() // Gleitende Durchschnitte
    {
        var sequence = Enumerable.Range(0, 1000).Select(x => (int)(Math.Sqrt(x) * 100)).ToArray();
        for (int start = 0; start < sequence.Length; start += 100)
        {
            var r = start..(start + 10);
            var (min, max, average) = MovingAverage(sequence, r);
            Console.WriteLine($"From {r.Start} to {r.End}:  \tMin: {min},\tMax:{max},\tAverage:{average}");
        }
        for (int start = 0; start < sequence.Length; start += 100)
        {
            var r = ^(start + 10)..^start;
            var (min, max, average) = MovingAverage(sequence, r);
            Console.WriteLine($"From {r.Start} to {r.End}:  \tMin: {min},\tMax:{max},\tAverage:{average}");
        }
        // static Local function
        static (int min, int max, double average) MovingAverage(int[] subSequence, Range range) =>
            (
            subSequence[range].Min(),
            subSequence[range].Max(),
            subSequence[range].Average()
            );
    }
}
