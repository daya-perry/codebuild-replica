using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyApp;

namespace MyApp.Tests;

[TestClass]
public class IntegrationTests
{
    private Calculator _calculator = new();

    [TestMethod]
    [TestProperty("Type", "Integration")]
    public void Calculator_MultipleOperations_WorksTogether()
    {
        TestContext.WriteLine("Testing multiple calculator operations");
        var result1 = _calculator.Add(10, 5);
        var result2 = _calculator.Multiply(result1, 2);
        Assert.AreEqual(30, result2);
        TestContext.WriteLine($"Final result: {result2}");
    }

    public TestContext TestContext { get; set; } = null!;
}
