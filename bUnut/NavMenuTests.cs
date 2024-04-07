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
public class NavMenuTests : TestContext
{
    [Fact]
    public void NavMenuComponentRendersCorrectly()
    {

        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "frontend", "wwwroot", "functions_config.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConfiguration>(config);

        // Act
        var cut = ctx.RenderComponent<frontend.Shared.NavMenu>();

        // Assert
        Assert.NotNull(cut);
        // Additional assertions as needed

        var links = cut.FindAll("a");
        Assert.NotNull(links);
        Assert.Equal(9, links.Count);
        Assert.Equal("Randomness", links[0].InnerHtml);
        Assert.Equal("<span class=\"oi oi-home\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Home\n            ", links[1].InnerHtml);
        Assert.Equal("<span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Fetch toss data\n            ", links[2].InnerHtml);
        Assert.Equal("<span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Fetch clustering data\n            ", links[3].InnerHtml);
        Assert.Equal("<span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Binary search\n            ", links[4].InnerHtml);
        Assert.Equal("<span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Cards\n            ", links[5].InnerHtml);
        Assert.Equal("<span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Domino\n            ", links[6].InnerHtml);
        Assert.Equal("<span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Socks\n            ", links[7].InnerHtml);
        Assert.Equal("<span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Secret code\n            ", links[8].InnerHtml);

        var menus = cut.FindAll("div");
        Assert.NotNull(menus);
        Assert.Equal(11, menus.Count);
        Assert.Equal("<div class=\"container-fluid\" b-szpj9ab99e=\"\"><a class=\"navbar-brand\" href=\"\" b-szpj9ab99e=\"\">Randomness</a>\n        <button title=\"Navigation menu\" class=\"navbar-toggler\" blazor:onclick=\"1\" b-szpj9ab99e=\"\"><span class=\"navbar-toggler-icon\" b-szpj9ab99e=\"\"></span></button></div>", menus[0].InnerHtml);
        Assert.Equal("<a class=\"navbar-brand\" href=\"\" b-szpj9ab99e=\"\">Randomness</a>\n        <button title=\"Navigation menu\" class=\"navbar-toggler\" blazor:onclick=\"1\" b-szpj9ab99e=\"\"><span class=\"navbar-toggler-icon\" b-szpj9ab99e=\"\"></span></button>", menus[1].InnerHtml);
        //Assert.Equal("<nav class=\"flex-column\" b-szpj9ab99e=\"\"><div class=\"nav-item px-3\" b-szpj9ab99e=\"\"><a href=\"\" class=\"nav-link active\" aria-current=\"page\"><span class=\"oi oi-home\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Home\n            </a></div>\n        <div class=\"nav-item px-3\" b-szpj9ab99e=\"\"><a href=\"fetchtoss\" class=\"nav-link\"><span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Fetch toss data\n            </a></div>\n        <div class=\"nav-item px-3\" b-szpj9ab99e=\"\"><a href=\"fetchpoints\" class=\"nav-link\"><span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Fetch clustering data\n            </a></div>\n        <div class=\"nav-item px-3\" b-szpj9ab99e=\"\"><a href=\"binary-search\" class=\"nav-link\"><span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Binary search\n            </a></div>\n        <div class=\"nav-item px-3\" b-szpj9ab99e=\"\"><a href=\"deck-cards\" class=\"nav-link\"><span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Cards\n           ...", menus[2].InnerHtml);
        Assert.Equal("<a href=\"\" class=\"nav-link active\" aria-current=\"page\"><span class=\"oi oi-home\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Home\n            </a>", menus[3].InnerHtml);
        Assert.Equal("<a href=\"fetchtoss\" class=\"nav-link\"><span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Fetch toss data\n            </a>", menus[4].InnerHtml);
        Assert.Equal("<a href=\"fetchpoints\" class=\"nav-link\"><span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Fetch clustering data\n            </a>", menus[5].InnerHtml);
        Assert.Equal("<a href=\"binary-search\" class=\"nav-link\"><span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Binary search\n            </a>", menus[6].InnerHtml);
        Assert.Equal("<a href=\"deck-cards\" class=\"nav-link\"><span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Cards\n            </a>", menus[7].InnerHtml);
        Assert.Equal("<a href=\"domino\" class=\"nav-link\"><span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Domino\n            </a>", menus[8].InnerHtml);
        Assert.Equal("<a href=\"socks\" class=\"nav-link\"><span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Socks\n            </a>", menus[9].InnerHtml);
        Assert.Equal("<a href=\"secret-code\" class=\"nav-link\"><span class=\"oi oi-list-rich\" aria-hidden=\"true\" b-szpj9ab99e=\"\"></span> Secret code\n            </a>", menus[10].InnerHtml);
    }
}