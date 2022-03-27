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

Console.WriteLine("É HORA DO SHOW");
using var playwright = await Playwright.CreateAsync();
await using var browser =
    await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 1_000});
var context = await browser.NewContextAsync(new BrowserNewContextOptions { BaseURL = "https://maddas.com.br/" });
var page = await context.NewPageAsync();
page.SetDefaultTimeout(5_000);
page.SetDefaultNavigationTimeout(30_000);
await context.Tracing.StartAsync(new() { Screenshots = true, Snapshots = true });

try
{
    const string adicionarAoCarrinho = "text=Adicionar ao carrinho";
    foreach (var item in cardápio.Itens.Where(x => x.Quantidade > 1))
    {
        await page.GotoAsync($"busca?palavra={item.Porção.Nome}");

        var resultados = page.Locator(".box-produto");

        if (await resultados.CountAsync() == 1)
        {
            await resultados.SelecionarQuantidade(item.Quantidade);
            await page.ClickAsync(adicionarAoCarrinho);
        }
        else
        {
            var produto = page.Locator(".box-produto", new() { Has = page.Locator($"'{item.Porção.Nome}'") });

            await produto.SelecionarQuantidade(item.Quantidade);
            await produto.Locator(adicionarAoCarrinho).ClickAsync();
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
