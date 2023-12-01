using ClassLibrary1;
using Xunit.Abstractions;

namespace TestProject1
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            output = testOutputHelper;
        }
        [Fact]
        public void PointDemo()
        {
            Point.Demo();
            output.WriteLine("PointDemo");
        }
    }
}