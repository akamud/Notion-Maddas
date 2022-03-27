using Microsoft.Playwright;

namespace NotionMaddas;

public class AutomatizadorCompra
{
    private IBrowserContext _context;
    private IPage _page;
    private const string selectorAdicionarAoCarrinho = "text=Adicionar ao carrinho";
    private const string selectorBoxProduto = ".box-produto";

    public async Task Inicializar()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser =
            await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        _context = await browser.NewContextAsync(new BrowserNewContextOptions { BaseURL = "https://maddas.com.br/" });
        _page = await _context.NewPageAsync();
        _page.SetDefaultTimeout(5_000);
        _page.SetDefaultNavigationTimeout(30_000);
    }

    public async Task ExecutarCompra(Cardápio cardápio, bool tracing)
    {
        if (tracing)
            await _context.Tracing.StartAsync(new() { Screenshots = true, Snapshots = true });

        try
        {
            foreach (var (porção, quantidade) in cardápio.Itens)
            {
                await _page.GotoAsync($"busca?palavra={porção.Nome}");

                var produtosEncontrados = _page.Locator(selectorBoxProduto);

                if (await produtosEncontrados.CountAsync() == 1)
                {
                    await produtosEncontrados.SelecionarQuantidade(quantidade);
                    await produtosEncontrados.SelecionarPeso(porção.Peso);
                    await _page.ClickAsync(selectorAdicionarAoCarrinho);
                }
                else
                {
                    var produto = _page.Locator(selectorBoxProduto, new() { Has = _page.Locator($"'{porção.Nome}'") });

                    await produto.SelecionarQuantidade(quantidade);
                    await produto.SelecionarPeso(porção.Peso);
                    await produto.Locator(selectorAdicionarAoCarrinho).ClickAsync();
                }

                await _page.ClickAsync("text=Continuar comprando");
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
                await _context.Tracing.StopAsync(new TracingStopOptions { Path = "tracing.zip" });
        }
    }
}
