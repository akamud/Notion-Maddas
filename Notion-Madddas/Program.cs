using Microsoft.Playwright;
using NotionMaddas;
using NotionMaddas.Dominio;

var porções = await NotionParser.ObterPorções();

var cardápio = new Cardápio();
foreach (var porção in porções)
{
    cardápio.AdicionarItem(porção);
}

ConsoleDebugger.Imprimir(cardápio);

using var playwright = await Playwright.CreateAsync();
await using var browser =
    await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
var context = await browser.NewContextAsync(new BrowserNewContextOptions { BaseURL = "https://maddas.com.br/" });
var page = await context.NewPageAsync();
page.SetDefaultTimeout(5_000);
page.SetDefaultNavigationTimeout(30_000);
await context.Tracing.StartAsync(new() { Screenshots = true, Snapshots = true });

try
{
    Console.WriteLine("É HORA DO SHOW");
    const string adicionarAoCarrinho = "text=Adicionar ao carrinho";
    foreach (var item in cardápio.Itens)
    {
        await page.GotoAsync($"busca?palavra={item.Porção.Nome}");

        var resultados = await page.Locator(".box-produto").CountAsync();

        if (resultados == 1)
            await page.ClickAsync(adicionarAoCarrinho);
        else
        {
            await page.Locator(".box-produto", new() { Has = page.Locator($"'{item.Porção.Nome}'") })
                .Locator(adicionarAoCarrinho)
                .ClickAsync();
        }

        await page.ClickAsync("text=Continuar comprando");
    }
}
catch (Exception) { }
finally
{
    await context.Tracing.StopAsync(new TracingStopOptions { Path = "tracing.zip" });
}
Console.WriteLine("E POR HOJE É SÓ");

Console.ReadLine();
