using ClassLibrary1;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace UnitTestProjectFullFramework481
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Demo01_Point()
        {
            Point.Demo();
            Console.WriteLine("PointDemo");
        }
        [TestMethod]
        public void Demo02_User()
        {
            UserR.WithOperator();
        }
        [TestMethod]
        public void Demo03_Operators()
        {
            Operators.IfElseToBracesToTernaryOperatorToSwitchStatementToSwitchExpression();
        }
        [TestMethod]
        public void Demo04_Operators()
        {
            Operators.Demo();
        }
        [TestMethod]
        public void Demo05_Operators()
        {
            Operators.MovingAverage();
        }
        [TestMethod]
        public void Demo06_NET8()
        {
            NET8.NewFeatures();
        }
        [TestMethod]
        public void Demo07_NET8()
        {
            NET8.NewCollectionFeatures();
        }
        [TestMethod]
        public void Demo08_NullableReferenceTypeDemo()
        {
            NullableReferenceTypeDemo.Demo();
        }
        [TestMethod]
        public void Demo09_NET8()
        {
            NET8.DemoCollectionExpressions();
        }
        [TestMethod]
        public void Demo10_NET8()
        {
            NET8.DemoPrimaryCtorClasses();
        }
        [TestMethod]
        public void Demo11_Patterns()
        {
            Patterns.IsWeekDay(DateOnly.FromDateTime(DateTime.Now)).Should().BeTrue();
            // Patterns.IsWeekDay()
        }
        [TestMethod]
        public void Demo12_Patterns()
        {
            Patterns.TypePattern();
        }
        [TestMethod]
        public void Demo13_Patterns()
        {
            Patterns.ListPatterns();
            Patterns.ListPatterns2();
        }
        [TestMethod]
        public void Demo14_Patterns()
        {
            Patterns.SwitchPattern();
        }

        [TestMethod]
        [DataRow(12, true)]
        [DataRow(8, false)]
        public void Demo15_Patterns(int hour, bool expectedResult)
        {
            Patterns.IsArbeitsPausenZeit(new DateTime(2023, 12, 1, hour, 0, 0))
                .Should().Be(expectedResult);
        }
        [TestMethod]
        public void Demo16_Patterns()
        {
            Patterns.ExtendedPropertyPattern();
        }
        [TestMethod]
        public async Task<string> Demo17_AsyncAwait()
        {
            return await new AsyncAwait().RunSomethingGood();
            // var result = new AsyncAwait().RunSomethingGood().Result;
            // return result;
        }
        [TestMethod]
        public void Demo17_Threads()
        {
            Threads.Demo();
        }
        [TestMethod]
        public void Demo18_AsyncAwait()
        {
            new AsyncAwait().RunSomethingBad();
        }
        [TestMethod]
        public void Demo19_AsyncAwait()
        {
            // BAD
            var number = new Repository().GetIntAsync().Result; // BAD: can cause deadlocks, but .net core fixes this, will work. Next Problem: potentail threadpool issues, waste resources. better: await always!
        }
        [TestMethod]
        public async Task<int> Demo20_AsyncAwait()
        {
            // GOOD
            return await new Repository().GetIntAsync();
        }
        [TestMethod]
        public void Demo21_Performance()
        {
            new Performance().DemoStackallocAndSpanBegin();
        }
        [TestMethod]
        public void Demo22_Performance()
        {
            var result = Performance.CalculateAverageAge();
            Console.WriteLine(result.ToString());
        }
        [TestMethod]
        public void Demo23_Performance()
        {
            var result = Performance.CalculateAverageAgeWithSpan();
            Console.WriteLine(result.ToString());
        }

        [TestMethod]
        public void TestSpan()
        {
            Span<int> end = [5, 6, 7];
            end.Length.Should().Be(3);
        }
    }
}
