using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyApp;
using System.Diagnostics;

namespace MyApp.Tests;

[TestClass]
public class PerformanceTests
{
    private Calculator _calculator = new();

    [TestMethod]
    [TestProperty("Type", "Performance")]
    public void Calculator_PerformanceTest_CompletesInTime()
    {
        TestContext.WriteLine("Starting performance test");
        var stopwatch = Stopwatch.StartNew();
        
        for (int i = 0; i < 1000000; i++)
        {
            _calculator.Add(i, i + 1);
        }
        
        stopwatch.Stop();
        TestContext.WriteLine($"Performance test completed in {stopwatch.ElapsedMilliseconds}ms");
        Assert.IsTrue(stopwatch.ElapsedMilliseconds < 5000, "Performance test should complete within 5 seconds");
    }

    public TestContext TestContext { get; set; } = null!;
}
