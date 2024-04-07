using System.Reflection;
using System;
using AngleSharp.Dom;
using frontend;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace bUnut;

/// <summary>
/// These tests are written entirely in C#.
/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
public class HomeTests : TestContext
{
    [Fact]
    public void HomeComponentRendersCorrectly()
    {

        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);

        // Act
        var cut = ctx.RenderComponent<frontend.Pages.Index>();

        // Assert
        Assert.NotNull(cut);
        // Additional assertions as needed

        var header = cut.Find("h1");

        Assert.NotNull(header);
        Assert.Equal("Hello, world!", header.InnerHtml);

        cut.MarkupMatches("\r\n\r\n<h1>Hello, world!</h1>\r\n\r\nWelcome to out randomness testing app. Use the menu to navigate.");
    }
}
