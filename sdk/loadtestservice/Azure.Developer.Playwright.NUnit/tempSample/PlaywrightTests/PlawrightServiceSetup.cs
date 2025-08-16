using System.Runtime.InteropServices;
using Azure.Developer.Playwright;
using Azure.Developer.Playwright.NUnit;
using Azure.Identity;
using NUnit.Framework;

namespace PlaywrightTests;     // Remember to change this as per your project namespace

[SetUpFixture]
public class PlaywrightServiceNUnitSetup : PlaywrightServiceBrowserNUnit
{
    public PlaywrightServiceNUnitSetup() : base(
        credential: new DefaultAzureCredential(),
        options: new PlaywrightServiceBrowserClientOptions
        {

            UseCloudHostedBrowsers = true,
            OS = OSPlatform.Linux,
            ExposeNetwork = "<loopback>",
            RunId = Guid.NewGuid().ToString(),
            //   RunName = "Github action test",
            ServiceAuth = ServiceAuthType.EntraId
        }
    )
    { }
}
