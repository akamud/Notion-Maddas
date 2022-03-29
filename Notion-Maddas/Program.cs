using NotionMaddas;
using NotionMaddas.Dominio;
using Spectre.Console;

var databaseId = args[0].Split('=')[1];
var tracing = false;
if (args.Length > 1)
    bool.TryParse(args[1].Split('=')?[1], out tracing);

Cardápio? cardápio = null;
await AnsiConsole.Progress()
    .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new SpinnerColumn())
    .StartAsync(async contexto =>
    {
        var notionTask = contexto.AddTask($"{Emoji.Known.GlobeWithMeridians} Obtendo dados do Notion")
            .IsIndeterminate();
        var porções = await NotionParser.ObterPorções(databaseId);
        notionTask.IsIndeterminate(false);
        notionTask.Increment(100);
        notionTask.StopTask();

        cardápio = new Cardápio();
        foreach (var porção in porções)
        {
            cardápio.AdicionarItem(porção);
        }
    });

if (cardápio == null)
    return;

ConsoleDebugger.Imprimir(cardápio);

await AutomatizadorCompra.ExecutarCompra(cardápio, tracing);
