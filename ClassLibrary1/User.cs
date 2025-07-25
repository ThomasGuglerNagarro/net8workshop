 using System;

namespace ClassLibrary1;

internal record struct Person(string FirstName);
internal record class Person2(string FirstName);
internal record Person3(string FirstName); // implicit class

/// <summary>
/// Primary Ctor
/// </summary>
/// <param name="name"></param>
/// <param name="dateOfBirth"></param>
public class User(string name, DateOnly dateOfBirth, string something)
{
    private string otherProp;
    public string OtherProp
    {
        get => this.otherProp;
        set => this.otherProp = value ?? throw new ArgumentNullException(paramName: nameof(value), message: "Cannot set name to null");
    }
    // public string Name { get; } = name;
    public string Name { get; } = !string.IsNullOrWhiteSpace(name) ? name
        : throw new ArgumentException("Name darf nicht leer sein");

    public int Age { get; } = DateTime.Today.Year - dateOfBirth.Year;
    // public int Age2 => DateTime.Today.Year - dateOfBirth.Year;
    public string OutputOtherProp(User u)
    {
        Console.WriteLine(something);
        _ = u ?? throw new ArgumentNullException(); // discard, besser als "if null else,..
        return u.OtherProp;
    }
    public string? PropThatCanBeNull { get; set; } // nullable reference type

    public static void SetName()
    {
        var user = new User(String.Empty, DateOnly.MinValue, "something");
    }

    public static int GetNameLength(User user)
    {
        int? wert = default;
        if (wert.HasValue)
            wert = 5;

          _ = user.PropThatCanBeNull ?? throw new ArgumentNullException(); 
        return user.PropThatCanBeNull.Length; // warning, get rid of warning with "!" operator
        // return user?.PropThatCanBeNull?.Length ?? 0; // ?. operator and null colescing operator

        // property pattern matching fpr "not null check"
        // if (user?.PropThatCanBeNull is { Length: var length }) return length; 
        // return user?.PropThatCanBeNull is { Length: var length } ? length : 0;
    }

}

/// <summary>
/// Record != Primary Ctor
/// </summary>
/// <param name="Name"></param>
/// <param name="DateOfBirth"></param>
public record class UserR(string Name, DateOnly DateOfBirth)
{
    public static void WithOperator() // WithOperator
    {
        var user = new UserR("mame", DateOnly.MinValue);
        var user2 = user with { Name = "other name" }; // With not class!?
        string name = "test"; // Deconstruct
        (name, var dateOfBirth) = user2;
        Console.WriteLine(dateOfBirth);

        // Check Utils.cs
        var (year, _, _) = DateTime.Now;
    }
    // ~UserR() => Console.WriteLine("Destructor");
    public void Deconstruct(out string name, out DateOnly dateOfBirth)
    {
        name = Name;
        dateOfBirth = DateOfBirth;
    }
}



public class Workshop(string param1, string param2)
{
    static void Demo()
    {
        var w = new Workshop("a", "b");
    }
}
