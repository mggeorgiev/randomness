using System.Reflection;
using System;
using AngleSharp.Dom;
using frontend;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using Bunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace bUnut;

/// <summary>
/// These tests are written entirely in C#.
/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
public class BinarySearchTests : TestContext
{
    [Fact]
    public void BinarySearchComponentRendersCorrectly()
    {

        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);

        // Act
        var cut = ctx.RenderComponent<frontend.Pages.BinarySearch>();

        // Assert
        Assert.NotNull(cut);
        // Additional assertions as needed

        var header = cut.Find("h1");

        Assert.NotNull(header);
        Assert.Equal("Find a random number in a series betrween 1 and 100.", header.InnerHtml);

        var inputTextBox = cut.Find("input");
        Assert.NotNull(inputTextBox);
        Assert.Equal("0", inputTextBox.GetAttribute("value"));

        var buttonGuest = cut.Find("#Guess");
        Assert.NotNull(buttonGuest);
        Assert.Equal("Guess", buttonGuest.InnerHtml);
        Assert.Equal("btn btn-primary", buttonGuest.ClassName);

        var buttonNew = cut.Find("#New");
        Assert.NotNull(buttonNew);
        Assert.Equal("New", buttonNew.InnerHtml);
        Assert.Equal("btn btn-secondary", buttonNew.ClassName);

        // Assert that content of the paragraph shows counter at zero
        cut.Find("#Result").MarkupMatches("<label id=\"Result\">Result: </label>");
        cut.Find("#triesCounter").MarkupMatches("<label id=\"triesCounter\"># of tries: 0</label>");
    }

    [Fact]
    public void BinarySearchComponentHasTitle()
    {
        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);

        // Act
        var cut = ctx.RenderComponent<frontend.Pages.BinarySearch>();

        // Assert
        var header = cut.Find("h1");

        Assert.NotNull(header);
        Assert.Equal("Find a random number in a series betrween 1 and 100.", header.InnerHtml);
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
        var cut = ctx.RenderComponent<frontend.Pages.BinarySearch>();

        // Assert that content of the paragraph shows counter at zero
        cut.Find("#Result").MarkupMatches("<label id=\"Result\">Result: </label>");
        cut.Find("#triesCounter").MarkupMatches("<label id=\"triesCounter\"># of tries: 0</label>");
    }


    [Fact]
    public void ClickingGuessButtonIncrementsTries()
    {
        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);
        var cut = ctx.RenderComponent<frontend.Pages.BinarySearch>();

        // Act - click button to increment counter
        cut.Find("#Guess").Click();

        // Assert that the counter was incremented
        cut.Find("#Result").MarkupMatches("<label id=\"Result\">Result: go up</label>");
        cut.Find("#triesCounter").MarkupMatches("<label id=\"triesCounter\"># of tries: 1</label>");
    }

    [Fact]
    public async Task ClickingRessetButtonRessetsTries()
    {
        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);
        var cut = ctx.RenderComponent<frontend.Pages.BinarySearch>();

        // Act - click button to increment counter
        cut.Find("#Guess").Click();
        await Task.Delay(100); // Adjust the delay time as needed

        // Assert that the counter was incremented
        cut.Find("#Result").MarkupMatches("<label id=\"Result\">Result: go up</label>");
        cut.Find("#triesCounter").MarkupMatches("<label id=\"triesCounter\"># of tries: 1</label>");

        // Act - click button to resset counter
        cut.Find("#New").Click();
        await Task.Delay(100); // Adjust the delay time as needed

        // Assert that content of the paragraph shows ressets counter at 0 and label
        cut.Find("#Result").MarkupMatches("<label id=\"Result\">Result: </label>");
        cut.Find("#triesCounter").MarkupMatches("<label id=\"triesCounter\"># of tries: 0</label>");
    }

    [Fact]
    public void ClickingGuessButtonAddsLogs()
    {
        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);
        var cut = ctx.RenderComponent<frontend.Pages.BinarySearch>();

        string[,] expectedValues = new string[2, 2] // Assuming 9 rows and 6 columns
        {
            { "#", "1"}, // Expected values for the first row
            { "", "0"}, // Expected values for the second row
        };

        // Act - click button to increment counter
        cut.Find("#Guess").Click();
        var table = cut.Find("table");
        var rows = cut.FindAll("tr");


        // Assert that the counter was incremented
        Assert.NotNull(table);
        Assert.Equal(2, rows.Count);

        for (int i = 0; i < rows.Count; i++)
        {
            var selector = "td";
            if (i == 0) selector = "th";

            var row = rows[i];


            // Do something with each row element
            Assert.NotNull(row);

            var cells = row.QuerySelectorAll(selector);

            // Assert
            Assert.NotNull(cells);
            Assert.NotEmpty(cells);

            for (int n = 0; n < cells.Length; n++)
            {
                Assert.Equal(expectedValues[i, n], cells[n].InnerHtml);
            }
        }
    }

    [Fact]
    public async Task ClickingResetButtonClearsLogs()
    {
        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);
        var cut = ctx.RenderComponent<frontend.Pages.BinarySearch>();

        // Act - click button to increment counter
        cut.Find("#Guess").Click();

        await Task.Delay(100); // Adjust the delay time as needed

        var table = cut.Find("table");
        var rows = cut.FindAll("tr");
        Assert.NotNull(table);
        Assert.Equal(2, rows.Count);

        //Resset the log
        cut.Find("#New").Click();

        await Task.Delay(100); // Adjust the delay time as needed

        // Assert that the counter was incremented
        Assert.DoesNotContain("table", cut.Markup);
    }
}