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
using System.ComponentModel;

namespace bUnut;

/// <summary>
/// These tests are written entirely in C#.
/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
public class BurglarProblemTests : TestContext
{
    [Fact]
    public void BurglarProblemComponentRendersCorrectly()
    {
        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);

        // Act
        var cut = ctx.RenderComponent<frontend.Pages.BurglarProblem>();

        // Assert
        Assert.NotNull(cut);
        // Additional assertions as needed

        var header = cut.Find("h1");

        Assert.NotNull(header);
        Assert.Equal("The problem of quiting while one is still ahead.", header.InnerHtml);

        var inputTextBox = cut.Find("input");
        Assert.NotNull(inputTextBox);
        Assert.Equal(null, inputTextBox.GetAttribute("value"));

        var buttonGuest = cut.Find("#Answer");
        Assert.NotNull(buttonGuest);
        Assert.Equal("Answer", buttonGuest.InnerHtml);
        Assert.Equal("btn btn-primary", buttonGuest.ClassName);

        var buttonNew = cut.Find("#New");
        Assert.NotNull(buttonNew);
        Assert.Equal("New", buttonNew.InnerHtml);
        Assert.Equal("btn btn-secondary", buttonNew.ClassName);

        // Assert that content of the paragraph shows counter at zero
        cut.Find("#Result").MarkupMatches("<label id=\"Result\">Answer: </label>");

        // Act
        var paragraphElement = cut.Find("p");
        var paragraphContent = paragraphElement.TextContent;
        var skillValue = cut.Find("b").TextContent;

        // Assert
        Assert.Contains("source", paragraphContent);
        Assert.Contains("%", paragraphContent);
        Assert.Contains("how many robberies should one undertake", paragraphContent);
    }

    [Fact]
    public void BurglarProblemComponentHasTitle()
    {
        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);

        // Act
        var cut = ctx.RenderComponent<frontend.Pages.BurglarProblem>();

        // Assert
        Assert.NotNull(cut);
        // Additional assertions as needed

        var header = cut.Find("h1");

        Assert.NotNull(header);
        Assert.Equal("The problem of quiting while one is still ahead.", header.InnerHtml);
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
        var cut = ctx.RenderComponent<frontend.Pages.BurglarProblem>();

        // Assert that content of the paragraph shows counter at zero
        cut.Find("#Result").MarkupMatches("<label id=\"Result\">Answer: </label>");
    }

    [Fact]
    public async Task ClickingAnswerButtonAddsAnswer()
    {
        // Arrange
        string initialAnswer = "<label id=\"Result\">Answer: </label>";
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);
        var cut = ctx.RenderComponent<frontend.Pages.BurglarProblem>();

        var result = cut.Find("#Result");
        cut.Find("#Result").MarkupMatches(initialAnswer);

        // Act
        var paragraphElement = cut.Find("p");
        var paragraphContent = paragraphElement.TextContent;
        var skillValueWithPercent = cut.Find("b").TextContent;

        // Remove the percent symbol
        var skillValue = int.Parse(skillValueWithPercent.Replace("%", ""));

        // Assert
        Assert.Contains("source", paragraphContent);
        Assert.Contains("%", paragraphContent);
        Assert.Contains("how many robberies should one undertake", paragraphContent);
        Assert.InRange(skillValue, 0, 100); // Assuming the skill value should be between 0 and 100

        var inputElement = cut.Find("input");
        Assert.NotNull(inputElement);
        inputElement.Change(skillValue / 10);

        // Act - click button to increment counter
        cut.Find("#Answer").Click();

        await Task.Delay(100); // Adjust the delay time as needed

        // Assert that content of the paragraph shows counter at zero
        result = cut.Find("#Result");

        Assert.NotNull(result);
        Assert.NotEqual(initialAnswer, result.ToString());
    }

    [Fact]
    public async Task CalculatesCorrerltyTheAnswer()
    {
        // Arrange
        string initialAnswer = "<label id=\"Result\">Answer: </label>";
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);
        var cut = ctx.RenderComponent<frontend.Pages.BurglarProblem>();

        // Act
        var paragraphElement = cut.Find("p");
        var paragraphContent = paragraphElement.TextContent;
        var skillValueWithPercent = cut.Find("b").TextContent;

        // Remove the percent symbol
        var skillValue = int.Parse(skillValueWithPercent.Replace("%", ""));
        int correctAnswer = ((int)skillValue / (100 - (int)skillValue));

        // Act: Try to find the details paragraph
        var detailsParagraphs = cut.FindAll("#details");

        // Assert: Check that no details paragraph is found
        Assert.Empty(detailsParagraphs);

        // Assert
        Assert.InRange(skillValue, 0, 100); // Assuming the skill value should be between 0 and 100

        //Act: Add the correct answer
        var inputElement = cut.Find("input");
        Assert.NotNull(inputElement);
        inputElement.Change(correctAnswer);

        // Act - click button to increment counter
        cut.Find("#Answer").Click();

        await Task.Delay(100); // Adjust the delay time as needed

        //Act - collect the result and the details.
        var result = cut.Find("#Result");
        detailsParagraphs = cut.FindAll("#details");

        // Assert that content of the paragraph shows counter at zero
        Assert.NotNull(result);
        Assert.NotEqual(initialAnswer, result.ToString());
        Assert.Equal("The answer is correct.", detailsParagraphs[0].InnerHtml);
    }

    [Fact]
    public async Task DisplaysErrorMesageWhenanswerIsTooHigh()
    {
        // Arrange
        string initialAnswer = "<label id=\"Result\">Answer: </label>";
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);
        var cut = ctx.RenderComponent<frontend.Pages.BurglarProblem>();

        // Act
        var paragraphElement = cut.Find("p");
        var paragraphContent = paragraphElement.TextContent;
        var skillValueWithPercent = cut.Find("b").TextContent;

        // Remove the percent symbol
        var skillValue = int.Parse(skillValueWithPercent.Replace("%", ""));
        int correctAnswer = ((int)skillValue / (100 - (int)skillValue));

        // Act: Try to find the details paragraph
        var detailsParagraphs = cut.FindAll("#details");

        // Assert: Check that no details paragraph is found
        Assert.Empty(detailsParagraphs);

        // Assert
        Assert.InRange(skillValue, 0, 100); // Assuming the skill value should be between 0 and 100

        //Act: Add the correct answer
        var inputElement = cut.Find("input");
        Assert.NotNull(inputElement);
        inputElement.Change(correctAnswer+2);

        // Act - click button to increment counter
        cut.Find("#Answer").Click();

        await Task.Delay(100); // Adjust the delay time as needed

        //Act - collect the result and the details.
        var result = cut.Find("#Result");
        detailsParagraphs = cut.FindAll("#details");

        // Assert that content of the paragraph shows counter at zero
        Assert.NotNull(result);
        Assert.NotEqual(initialAnswer, result.ToString());
        Assert.Equal("The answer is higher that the correct one.", detailsParagraphs[0].InnerHtml);
    }

    [Fact]
    public async Task DisplaysErrorMesageWhenanswerIsTooLow()
    {
        // Arrange
        string initialAnswer = "<label id=\"Result\">Answer: </label>";
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);
        var cut = ctx.RenderComponent<frontend.Pages.BurglarProblem>();

        // Act
        var paragraphElement = cut.Find("p");
        var paragraphContent = paragraphElement.TextContent;
        var skillValueWithPercent = cut.Find("b").TextContent;

        // Remove the percent symbol
        var skillValue = int.Parse(skillValueWithPercent.Replace("%", ""));
        int correctAnswer = ((int)skillValue / (100 - (int)skillValue));

        // Act: Try to find the details paragraph
        var detailsParagraphs = cut.FindAll("#details");

        // Assert: Check that no details paragraph is found
        Assert.Empty(detailsParagraphs);

        // Assert
        Assert.InRange(skillValue, 0, 100); // Assuming the skill value should be between 0 and 100

        //Act: Add the correct answer
        var inputElement = cut.Find("input");
        Assert.NotNull(inputElement);
        inputElement.Change(correctAnswer - 2);

        // Act - click button to increment counter
        cut.Find("#Answer").Click();

        await Task.Delay(100); // Adjust the delay time as needed

        //Act - collect the result and the details.
        var result = cut.Find("#Result");
        detailsParagraphs = cut.FindAll("#details");

        // Assert that content of the paragraph shows counter at zero
        Assert.NotNull(result);
        Assert.NotEqual(initialAnswer, result.ToString());
        Assert.Equal("The answer is lower that the correct one.", detailsParagraphs[0].InnerHtml);
    }

    [Fact]
    public async Task ClickingResetButtonClearsDetails()
    {
        // Arrange
        string initialAnswer = "<label id=\"Result\">Answer: </label>";
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);
        var cut = ctx.RenderComponent<frontend.Pages.BurglarProblem>();

        var result = cut.Find("#Result");
        cut.Find("#Result").MarkupMatches(initialAnswer);

        // Act: Try to find the details paragraph
        var detailsParagraphs = cut.FindAll("#details");

        // Assert: Check that no details paragraph is found
        Assert.Empty(detailsParagraphs);

        var inputElement = cut.Find("input");
        Assert.NotNull(inputElement);
        inputElement.Change("0");

        // Act - click button to increment counter
        cut.Find("#Answer").Click();

        await Task.Delay(100); // Adjust the delay time as needed

        // Assert that content of the paragraph shows counter at zero
        result = cut.Find("#Result");
        var detailsParagraph = cut.Find("#details");

        Assert.NotNull(result);
        Assert.NotEqual(initialAnswer, result.ToString());

        Assert.NotNull(detailsParagraph);

        //Resset the log
        cut.Find("#New").Click();

        await Task.Delay(100); // Adjust the delay time as needed

        // Assert that the answer ressets to the initial
        cut.Find("#Result").MarkupMatches(initialAnswer);

        // Act: Try to find the details paragraph
        detailsParagraphs = cut.FindAll("#details");

        // Assert: Check that no details paragraph is found
        Assert.Empty(detailsParagraphs);
    }
}