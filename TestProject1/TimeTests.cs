using ClassLibrary1;
using NSubstitute;

namespace TestProject1;

public class TimeTests(ITestOutputHelper testOutputHelper)
{
    [Theory]
    [InlineData("2023-01-30")]
    [InlineData("2023-02-01")]
    [InlineData("2023-02-27")]
    [InlineData("2023-03-01")]
    [InlineData("2023-04-30")]
    [InlineData("2023-09-30")]
    [InlineData("2023-12-31")]
    public void Is_Is_Not_Payday(string date)
    {
        // Arrange
        var startDateTime = new DateTimeOffset(DateOnly.Parse(date), TimeOnly.MinValue, TimeSpan.Zero);
        // var timeProvider = new FakeTimeProvider(startDateTime);
        // var mock = new Mock<TimeProvider>();
        var mocktimeProvider = Substitute.For<TimeProvider>();
        mocktimeProvider.GetUtcNow().Returns(startDateTime);
        var calculator = new PaydayCalculator(mocktimeProvider);
        // Act
        var actual = calculator.IsPayday();
        // Assert
        actual.Should().BeFalse();
    }
}
