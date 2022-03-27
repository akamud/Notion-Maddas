using Microsoft.Playwright;

namespace NotionMaddas;

public class AutomatizadorCompra
{
    private const string selectorAdicionarAoCarrinho = "text=Adicionar ao carrinho";
    private const string selectorBoxProduto = ".box-produto";

    public async Task ExecutarCompra(Cardápio cardápio, bool tracing)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser =
            await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions {Headless = false});
        var context = await browser.NewContextAsync(new BrowserNewContextOptions {BaseURL = "https://maddas.com.br/"});
        var page = await context.NewPageAsync();
        page.SetDefaultTimeout(5_000);
        page.SetDefaultNavigationTimeout(30_000);
        if (tracing)
            await context.Tracing.StartAsync(new() {Screenshots = true, Snapshots = true});

        try
        {
            foreach (var (porção, quantidade) in cardápio.Itens)
            {
                await page.GotoAsync($"busca?palavra={porção.Nome}");

                var produtosEncontrados = page.Locator(selectorBoxProduto);

                if (await produtosEncontrados.CountAsync() == 1)
                {
                    await produtosEncontrados.SelecionarQuantidade(quantidade);
                    await produtosEncontrados.SelecionarPeso(porção.Peso);
                    await page.ClickAsync(selectorAdicionarAoCarrinho);
                }
                else
                {
                    var produto = page.Locator(selectorBoxProduto, new() {Has = page.Locator($"'{porção.Nome}'")});

                    await produto.SelecionarQuantidade(quantidade);
                    await produto.SelecionarPeso(porção.Peso);
                    await produto.Locator(selectorAdicionarAoCarrinho).ClickAsync();
                }

                await page.ClickAsync("text=Continuar comprando");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro na busca: {e.Message}");
            Console.WriteLine(e.StackTrace);
        }
        finally
        {
            if (tracing)
                await context.Tracing.StopAsync(new TracingStopOptions {Path = "tracing.zip"});
        }
    }
}