using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyApp;

namespace MyApp.Tests;

[TestClass]
public class E2EIntegrationTests
{
    private Calculator _calculator = new();

    [TestMethod]
    [TestProperty("Type", "E2E-Integration")]
    public void Calculator_EndToEndScenario_Success()
    {
        TestContext.WriteLine("Starting E2E test scenario");
        TestContext.WriteLine("Step 1: Adding numbers");
        var step1 = _calculator.Add(100, 50);
        TestContext.WriteLine($"Step 1 result: {step1}");
        
        TestContext.WriteLine("Step 2: Dividing result");
        var step2 = _calculator.Divide(step1, 3);
        TestContext.WriteLine($"Step 2 result: {step2}");
        
        Assert.AreEqual(50, step2);
        TestContext.WriteLine("E2E test completed successfully");
    }

    [TestMethod]
    [TestProperty("Type", "E2E-Integration")]
    public void Calculator_ErrorHandling_ThrowsException()
    {
        TestContext.WriteLine("Testing error handling in E2E scenario");
        Assert.ThrowsException<DivideByZeroException>(() => _calculator.Divide(10, 0));
        TestContext.WriteLine("Exception handling test passed");
    }

    public TestContext TestContext { get; set; } = null!;
}
