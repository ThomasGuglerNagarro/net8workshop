namespace ClassLibrary1;

public class SomeOtherFeaturesToShow
{
    public void LambdaImprovementsDemo()
    {
        // ALT Func<string> hello = () => "blaaaa";
        var hello = () => "blaaaa";
        Console.WriteLine(hello);

        // C#9:
        Func<string, int> parse = (string number) => int.Parse(number);
        // c#10:
        var parse2 = (string number) => int.Parse(number);
    }
}

// C#11
public class GenericAttribute<T> : Attribute
{
}

public class StringLiterals
{
    public void Utf8StringLiteralsSample()
    {
        ReadOnlySpan<byte> data = "Thomas Gugler"u8;
        var space = data.IndexOf(" "u8);
        var first = data[..space];
        var second = data[++space..];
    }

    public string RawStringLiterals()
    {
        var xml = $"""
<?xml version='1.0' encoding='utf-8'?>  
<configuration>  
  <connectionStrings>  
    <clear />  
    <add name="Name"
     providerName="System.Data.ProviderName"
     connectionString="Valid Connection String;" />  
  </connectionStrings>  
</configuration>
""";
        return xml;
    }
}

/*
// C#11: static abstract members: not possible with .netstandard2.0
interface IAsyncFactory<TSelf>
{
    static abstract Task<TSelf> CreateAsync();
}

class PersonSample : IAsyncFactory<PersonSample>
{
    public static async Task<PersonSample> CreateAsync()
    {
        await Task.Delay(1000);
        return new();
    }
}
*/
