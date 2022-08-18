using Shouldly;
using Xunit;

namespace Worker.Domain.UnitTests;

public class CalculatorTests : UnitTestBase<Calculator>
{
    [Theory]
    [InlineData("CALCULATE: 0", 0)]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("1+1", 2)]
    [InlineData("2+2", 4)]
    [InlineData("2+2+2+2", 8)]
    [InlineData("1*1", 1)]
    [InlineData("2*3", 6)]
    [InlineData("2+2*3", 8)]
    [InlineData("2*3+2", 8)]
    [InlineData("1-1", 0)]
    [InlineData("-1+1", 0)]
    [InlineData("2*-1", -2)]
    [InlineData("2/2", 1)]
    [InlineData("2/-1", -2)]
    public void When_calculating(string calculation, int expected)
    {
        var result = Because(() => ClassUnderTest.Calculate(calculation));
        
        It("calculates the expected result", () => result.ShouldBe(expected));
    }
}
