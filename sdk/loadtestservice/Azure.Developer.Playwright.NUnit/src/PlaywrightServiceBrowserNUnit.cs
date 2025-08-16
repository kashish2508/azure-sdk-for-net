// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Azure.Core;
using System;

namespace Azure.Developer.Playwright.NUnit;

/// <summary>
/// NUnit setup fixture to initialize Playwright Service.
/// </summary>
[SetUpFixture]
public class PlaywrightServiceBrowserNUnit : PlaywrightServiceBrowserClient
{
    private readonly PlaywrightServiceBrowserClientOptions _options;
    private static NUnitLogger nunitLogger { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaywrightServiceBrowserNUnit"/> class.
    /// </summary>
    public PlaywrightServiceBrowserNUnit() : this(
        options: new PlaywrightServiceBrowserClientOptions()
        {
            Logger = nunitLogger
        }
    )
    {
        nunitLogger.LogInformation("[NUNIT] Default constructor called");
        // no-op
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaywrightServiceBrowserNUnit"/> class.
    /// </summary>
    /// <param name="credential">The token credential.</param>
    public PlaywrightServiceBrowserNUnit(TokenCredential credential) : this(
        options: new PlaywrightServiceBrowserClientOptions()
        {
            Logger = nunitLogger
        },
        credential: credential
    )
    {
        nunitLogger.LogInformation("[NUNIT] Constructor with credential called");
        // no-op
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaywrightServiceBrowserNUnit"/> class.
    /// </summary>
    /// <param name="options">Client options for PlaywrightBrowserClient.</param>
    public PlaywrightServiceBrowserNUnit(PlaywrightServiceBrowserClientOptions options) : base(
        options: options
    )
    {
        nunitLogger.LogInformation("[NUNIT] Constructor with options called");
        _options = options;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaywrightServiceBrowserNUnit"/> class.
    /// </summary>
    /// <param name="credential">The token credential.</param>
    /// <param name="options">Client options for PlaywrightBrowserClient.</param>
    public PlaywrightServiceBrowserNUnit(TokenCredential credential, PlaywrightServiceBrowserClientOptions options) : base(
        credential: credential,
        options: InjectNUnitLogger(options)
    )
    {
        nunitLogger.LogInformation("[NUNIT] Constructor with credential and options called");
        _options = options;
    }

    /// <summary>
    /// Setup the resources utilized by Playwright Browser client.
    /// </summary>
    /// <returns></returns>
    [OneTimeSetUp]
    public async Task InitializeAsync()
    {
        nunitLogger.LogInformation("[NUNIT] OneTimeSetUp InitializeAsync called");
        if (!_options.UseCloudHostedBrowsers)
        {
            nunitLogger.LogInformation("[NUNIT] Cloud hosted browsers disabled, exiting initialization");
            return;
        }
        nunitLogger.LogInformation("[NUNIT] Running tests using Azure Playwright service");
        nunitLogger.LogInformation("\nRunning kashish using Azure Playwright service.\n");

        nunitLogger.LogInformation("[NUNIT] Calling base.InitializeAsync() to set up authentication and create test run");
        await base.InitializeAsync().ConfigureAwait(false);
        nunitLogger.LogInformation("[NUNIT] InitializeAsync completed");
    }

    /// <summary>
    /// Tear down resources utilized by Playwright Browser client.
    /// </summary>
    [OneTimeTearDown]
    public override async Task DisposeAsync()
    {
        nunitLogger.LogInformation("[NUNIT] OneTimeTearDown DisposeAsync called");
        nunitLogger.LogInformation("[NUNIT] Calling base.DisposeAsync() to clean up resources");
        await base.DisposeAsync().ConfigureAwait(false);
        nunitLogger.LogInformation("[NUNIT] DisposeAsync completed");
    }

    private static PlaywrightServiceBrowserClientOptions InjectNUnitLogger(PlaywrightServiceBrowserClientOptions options)
    {
        nunitLogger.LogInformation("[NUNIT] InjectNUnitLogger called");
        if (options.Logger == null)
        {
            nunitLogger.LogInformation("[NUNIT] Injecting NUnit logger into options");
            options.Logger = nunitLogger;
        }
        else
        {
            nunitLogger.LogInformation("[NUNIT] Options already has a logger, not injecting NUnit logger");
        }
        return options;
    }
}
