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
public class SocksTests : TestContext
{
    [Fact]
    public void SocksComponentRendersCorrectly()
    {

        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);

        // Act
        var cut = ctx.RenderComponent<frontend.Pages.Socks>();

        // Assert
        Assert.NotNull(cut);
        // Additional assertions as needed

        var header = cut.Find("h1");

        Assert.NotNull(header);
        Assert.Equal("World of Engineering Quiz:", header.InnerHtml);

        var inputTextBox = cut.Find("input");
        Assert.NotNull(inputTextBox);
        Assert.Equal("0", inputTextBox.GetAttribute("value"));

        var button = cut.Find("button");
        Assert.NotNull(button);
        Assert.Equal("Get Data", button.InnerHtml);
        Assert.Equal("btn btn-primary", button.ClassName);
    }

    [Fact]
    public void SocksComponentHasTitle()
    {
        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);

        // Act
        var cut = ctx.RenderComponent<frontend.Pages.Socks>();

        // Assert
        var header = cut.Find("h1");

        Assert.NotNull(header);
        Assert.Equal("World of Engineering Quiz:", header.InnerHtml);
    }

    [Fact]
    public void TriesStartsAtZero()
    {
        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);

        // Act
        var cut = ctx.RenderComponent<frontend.Pages.Socks>();

        // Assert that content of the paragraph shows counter at zero
        cut.Find("label").MarkupMatches("<label># of tries: 0</label>");
    }


	[Fact]
	public void ClickingButtonIncrementsTries()
	{
        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);
        var cut = ctx.RenderComponent<frontend.Pages.Socks>();

        // Act - click button to increment counter
        cut.Find("button").Click();

        // Assert that the counter was incremented
        cut.Find("label").MarkupMatches("<label># of tries: 1</label>");
    }
}
