using Shouldly;
using Xunit;

namespace Worker.Domain.UnitTests;

public class CalculatorTests : UnitTestBase<Calculator>
{
    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("1+1", 2)]
    [InlineData("2+2", 4)]
    [InlineData("2+2+2+2", 8)]
    public void When_calculating(string calculation, int expected)
    {
        var result = Because(() => ClassUnderTest.Calculate(calculation));
        
        It("calculates the expected result", () => result.ShouldBe(expected));
    }
}
