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
        [Fact]
        public void Demo07_NET8()
        {
            NET8.NewCollectionFeatures();
        }
        [Fact]
        public void Demo08_NullableReferenceTypeDemo()
        {
            NullableReferenceTypeDemo.Demo();
        }
        [Fact]
        public void Demo09_NET8()
        {
            NET8.DemoCollectionExpressions();
        }
        [Fact]
        public void Demo10_NET8()
        {
            NET8.DemoPrimaryCtorClasses();
        }
    }
}