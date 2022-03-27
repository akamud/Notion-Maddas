using Microsoft.Playwright;

namespace NotionMaddas;

public static class LocatorExtensions
{
    public static async Task SelecionarQuantidade(this ILocator locator, int quantidade)
    {
        if (quantidade > 1)
            await locator.Locator("i:has-text(\"+\")").ClickAsync(new() { ClickCount = quantidade - 1 });
    }

    public static async Task SelecionarPeso(this ILocator locator, int? peso)
    {
        if (peso == null)
            return;

        await locator.Locator("select[name=\"gramas\"]")
            .SelectOptionAsync(new SelectOptionValue { Label = $"{peso}g" });
    }
}
