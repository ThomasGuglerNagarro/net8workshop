using System;

// [assembly: CLSCompliant(true)] => default(false)

namespace ClassLibrary1;

/// <summary>
/// https://learn.microsoft.com/de-de/dotnet/api/system.clscompliantattribute?view=net-8.0
/// https://learn.microsoft.com/en-us/dotnet/standard/language-independence
/// </summary>
public class PersonCLSCompliant
{
    private UInt16 personAge = 0;

    public UInt16 Age
    { get { return personAge; } }
}
// The attempt to compile the example displays the following compiler warning:
//    Public1.cs(10,18): warning CS3003: Type of 'Person.Age' is not CLS-compliant
