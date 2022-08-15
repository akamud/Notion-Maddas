using Microsoft.Playwright;
using NotionMaddas.Dominio;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace NotionMaddas;

public static class AutomatizadorCompra
{
    private const string selectorAdicionarAoCarrinho = "text=Adicionar ao carrinho";
    private const string selectorBoxProduto = ".box-produto";

    public static async Task ExecutarCompra(Cardápio cardápio, bool tracing)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser =
            await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        var context =
            await browser.NewContextAsync(new BrowserNewContextOptions { BaseURL = "https://maddas.com.br/" });
        var page = await context.NewPageAsync();
        page.SetDefaultTimeout(5_000);
        page.SetDefaultNavigationTimeout(30_000);
        if (tracing)
            await context.Tracing.StartAsync(new() { Screenshots = true, Snapshots = true });

        await AnsiConsole.Progress()
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new SpinnerColumn())
            .StartAsync(async contexto =>
            {
                var carrinhoTask = contexto.AddTask($"{Emoji.Known.ShoppingCart} Fazendo carrinho", true,
                    cardápio.QuantidadeItens);

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
                            var produto = page.Locator(selectorBoxProduto,
                                new() { HasTextRegex = new Regex($"^{porção.Nome.Trim()}$", RegexOptions.IgnoreCase) });

                            await produto.SelecionarQuantidade(quantidade);
                            await produto.SelecionarPeso(porção.Peso);
                            await produto.Locator(selectorAdicionarAoCarrinho).ClickAsync();
                        }

                        await page.ClickAsync("text=Continuar comprando");
                        carrinhoTask.Increment(quantidade);
                    }
                }
                catch (Exception e)
                {
                    AnsiConsole.MarkupLine($"{Emoji.Known.Collision} Erro na busca:");
                    AnsiConsole.WriteException(e);
                }
                finally
                {
                    if (tracing)
                        await context.Tracing.StopAsync(new TracingStopOptions { Path = "tracing.zip" });

                    carrinhoTask.StopTask();
                }
            });

        AnsiConsole.MarkupLine($"{Emoji.Known.CreditCard} Prossiga com o pagamento");
        Console.ReadLine();
    }
}
