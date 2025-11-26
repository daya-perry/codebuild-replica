using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit;
using MyApp;
using Xunit.Abstractions;

namespace MyApp.Tests;

public static class XUnitLoggerExtensions
{
    public static ILoggingBuilder AddXUnit(this ILoggingBuilder builder, ITestOutputHelper testOutputHelper)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            builder.AddProvider(new XunitLoggerProvider(testOutputHelper));
        return builder;
    }

    public static ILoggerFactory CreateXUnitLoggerFactory(
        this ITestOutputHelper testOutputHelper,
        LogLevel logLevel = LogLevel.Debug)
    {
        return LoggerFactory.Create(builder => builder.SetMinimumLevel(logLevel).AddXUnit(testOutputHelper));
    }
}

public class XunitLoggerProvider : ILoggerProvider
{
    private readonly ITestOutputHelper testOutputHelper;

    public XunitLoggerProvider(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new XUnitLogger(testOutputHelper, categoryName);
    }

    public void Dispose()
    {
    }
}

public class XUnitLogger : ILogger
{
    private readonly string categoryName;
    private readonly string shortCategoryName;
    private readonly ITestOutputHelper testOutputHelper;

    public XUnitLogger(ITestOutputHelper testOutputHelper, string categoryName)
    {
        this.testOutputHelper = testOutputHelper;
        this.categoryName = categoryName;
        this.shortCategoryName = ShortenCategory();
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return state != null ? XUnitLogScope.Push(categoryName, state) : throw new ArgumentNullException(nameof(state));
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        string scopeInformation = GetScopeInformation();
        testOutputHelper.WriteLine($"{DateTime.Now:HH:mm:ss.fff} {shortCategoryName} {logLevel} {scopeInformation}: {formatter(state, exception)}");
        if (exception != null)
            testOutputHelper.WriteLine(exception.ToString());
    }

    private string ShortenCategory()
    {
        if (categoryName.Length < 30)
            return categoryName;
        string[] parts = categoryName.Split('.');
        if (parts.Length < 3)
            return categoryName;
        for (int i = 0; i < parts.Length - 2; i++)
            parts[i] = parts[i][0].ToString();
        return string.Join(".", parts);
    }

    private static string GetScopeInformation()
    {
        var sb = new StringBuilder();
        var scope = XUnitLogScope.Current;
        if (scope == null)
            return string.Empty;
        for (; scope != null; scope = scope.Parent)
            sb.Append($"| {scope}");
        sb.Append(" | ");
        return sb.ToString();
    }
}

public class XUnitLogScope : IDisposable
{
    private static readonly AsyncLocal<XUnitLogScope> _current = new();
    
    public static XUnitLogScope Current => _current.Value;
    public XUnitLogScope Parent { get; }
    public string CategoryName { get; }
    public object State { get; }

    private XUnitLogScope(string categoryName, object state)
    {
        CategoryName = categoryName;
        State = state;
        Parent = Current;
        _current.Value = this;
    }

    public static XUnitLogScope Push(string categoryName, object state)
    {
        return new XUnitLogScope(categoryName, state);
    }

    public void Dispose()
    {
        _current.Value = Parent;
    }

    public override string ToString()
    {
        return State?.ToString() ?? string.Empty;
    }
}