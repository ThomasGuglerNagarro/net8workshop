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
public class User(string name, DateOnly dateOfBirth)
{
    private string otherProp;
    public string OtherProp
    {
        get => this.otherProp;
        set => this.otherProp = value ?? throw new ArgumentNullException(paramName: nameof(value), message: "Cannot set name to null");
    }
    public string Name { get; } = name;
    public int Age { get; } = DateTime.Today.Year - dateOfBirth.Year;
    public string OutputOtherProp(User u)
    {
        _ = u ?? throw new ArgumentNullException(); // discard, besser als "if null else,..
        return u.OtherProp;
    }
    public string? PropThatCanBeNull { get; set; } // nullable reference type

    public static int GetNameLength(User user)
    {
        // null check? _ = user.PropThatCanBeNull ?? throw new ArgumentNullException(); 
        return user.PropThatCanBeNull.Length; // warning, get rid of warning with "!" operator
        // return user?.PropThatCanBeNull?.Length ?? 0; // ?. operator and null colescing operator

        // property pattern matching fpr "not null check"
        // V1 if (user?.PropThatCanBeNull is { Length: var length }) return length; 
        // V2 return user?.PropThatCanBeNull is { Length: var length } ? length : 0;
    }

}

/// <summary>
/// Record != Primary Ctor
/// </summary>
/// <param name="Name"></param>
/// <param name="DateOfBirth"></param>
public record UserR(string Name, DateOnly DateOfBirth)
{
    public static void WithOperator() // WithOperator
    {
        var user = new UserR("mame", DateOnly.MinValue);
        var user2 = user with { Name = "other name" };
        string name = "test"; // Deconstruct
        (name, var dateOfBirth) = user2;
        Console.WriteLine(dateOfBirth);

        // Check Utils.cs
        var (year, month, day) = DateTime.Now;
    }
    public void Deconstruct(out string name, out DateOnly dateOfBirth)
    {
        name = Name;
        dateOfBirth = DateOfBirth;
    }
}




