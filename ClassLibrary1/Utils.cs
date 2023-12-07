namespace ClassLibrary1;

public static class Utils
{
    public static void Deconstruct(this DateTime dateTime,
        out int year, out int month, out int day)
    {
        year = dateTime.Year; 
        month = dateTime.Month;
        day = dateTime.Day;
    }
}
