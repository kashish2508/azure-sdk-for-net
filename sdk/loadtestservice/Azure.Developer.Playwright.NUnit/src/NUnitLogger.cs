// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Azure.Developer.Playwright.NUnit;

internal class NUnitLogger : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        Console.WriteLine("[NUNIT-LOGGER] Log called with level: {0}", logLevel);
        if (!IsEnabled(logLevel))
        {
            Console.WriteLine("[NUNIT-LOGGER] Log level {0} not enabled", logLevel);
            return;
        }
        if (formatter == null)
        {
            Console.WriteLine("[NUNIT-LOGGER] Formatter is null, throwing exception");
            throw new ArgumentNullException(nameof(formatter));
        }
        string message = formatter(state, exception);
        Console.WriteLine("[NUNIT-LOGGER] Message: {0}", message);
        if (exception != null)
        {
            Console.WriteLine("[NUNIT-LOGGER] Exception: {0}", exception);
            message += $"\nException: {exception}";
        }
        Log(logLevel, message);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    IDisposable? ILogger.BeginScope<TState>(TState state)
    {
        return NullScope.Instance;
    }

    private static void Log(LogLevel level, string message)
    {
        Console.WriteLine("[NUNIT-LOGGER] Writing log to appropriate output");
        System.IO.TextWriter writer = level == LogLevel.Error || level == LogLevel.Warning || level == LogLevel.Critical ? Console.Error : Console.Out;
        writer.WriteLine($"{DateTime.Now} [{level}]: {message}");

        if (level == LogLevel.Debug)
        {
            Console.WriteLine("[NUNIT-LOGGER] Writing to TestContext.WriteLine");
            TestContext.WriteLine($"[AzurePlaywright-NUnit]: {message}");
        }
        else if (level == LogLevel.Error || level == LogLevel.Critical)
        {
            Console.WriteLine("[NUNIT-LOGGER] Writing to TestContext.Error");
            TestContext.Error.WriteLine($"[AzurePlaywright-NUnit]: {message}");
        }
        else
        {
            Console.WriteLine("[NUNIT-LOGGER] Writing to TestContext.Progress");
            TestContext.Progress.WriteLine($"[AzurePlaywright-NUnit]: {message}");
        }
    }
};

internal class NullScope : IDisposable
{
    public static readonly NullScope Instance = new();

    private NullScope() { }

    public void Dispose()
    {
        // No operation
    }
}
