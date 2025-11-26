using Xunit;
using Xunit.Abstractions;
using MyApp;

namespace MyApp.Tests;

public class UnitTests
{
    private readonly Calculator _calculator;
    private readonly ITestOutputHelper _output;

    public UnitTests(ITestOutputHelper output)
    {
        _calculator = new Calculator();
        _output = output;
    }

    [Fact]
    [Trait("Type", "Unit")]
    public void Add_TwoNumbers_ReturnsSum()
    {
        _output.WriteLine("Testing addition of 2 + 3");
        var result = _calculator.Add(2, 3);
        Assert.Equal(5, result);
        _output.WriteLine($"Result: {result}");
    }

    [Fact]
    [Trait("Type", "Unit")]
    public void Subtract_TwoNumbers_ReturnsDifference()
    {
        _output.WriteLine("Testing subtraction of 5 - 3");
        var result = _calculator.Subtract(5, 3);
        Assert.Equal(2, result);
        _output.WriteLine($"Result: {result}");
    }
}
