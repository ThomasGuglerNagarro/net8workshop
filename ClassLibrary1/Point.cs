using System.Diagnostics;
// 1: Convert to file-scoped namespace
namespace ClassLibrary1;

public readonly struct Point // : IEquatable<Point>
{
    public static void Demo()
    {
        var p1 = new Point(1, 2);
        var p2 = new Point(1, 2);
        Debug.WriteLine(p1.Equals(p2));
        Console.WriteLine("sdfsdf");
        // p1.Test(null);
    }
    public readonly double Distance => Math.Sqrt(X * X + Y * Y);

    public int X { get; } // implicit readonly 
    public int Y { get; }
   /* public Point(int x, int y)
    {
        X = x;
        Y = y;
    } */
    // V2 with tuples
    public Point(int x, int y) => (X, Y) = (x, y);
    public static bool operator ==(Point a, Point b) => (a.X, a.Y) == (b.X, b.Y);
    public static bool operator !=(Point a, Point b) => (a.X, a.Y) != (b.X, b.Y);
    /*  public override bool Equals(object? obj)
      {
          if (obj is Point)
          {
              var otherPt = (Point)obj;
              return this == otherPt;
          }
          else
          {
              return false;
          }
      }*/
    public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
    // Equals new1
    public override bool Equals(object? obj) => obj is Point p && base.Equals(p); // Pattern matching
                                                                                  // Equals new2
                                                                                  // public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);
}
public record struct PointR(int X, int Y);
// public int X { get; set; }
// public int Y { get; set; }
