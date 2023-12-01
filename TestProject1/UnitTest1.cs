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
        public void Demo01_Point()
        {
            Point.Demo();
            output.WriteLine("PointDemo");
        }
        [Fact]
        public void Demo02_User()
        {
            UserR.WithOperator();
        }
        [Fact]
        public void Demo03_Operators()
        {
            Operators.IfElseToBracesToTernaryOperatorToSwitchStatementToSwitchExpression();
        }
        [Fact]
        public void Demo04_Operators()
        {
            Operators.Demo();
        }
        [Fact]
        public void Demo05_Operators()
        {
            Operators.MovingAverage();
        }
        [Fact]
        public void Demo06_NET8()
        {
            NET8.NewFeatures();
        }
    }
}