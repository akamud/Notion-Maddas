using NotionMaddas;
using NotionMaddas.Dominio;
using Spectre.Console;

await AnsiConsole.Progress()
    .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new SpinnerColumn())
    .StartAsync(async contexto =>
    {
        var notionTask = contexto.AddTask("Obtendo dados do Notion").IsIndeterminate();
        var porções = await NotionParser.ObterPorções();
        notionTask.StopTask();

        var cardápio = new Cardápio();
        foreach (var porção in porções)
        {
            cardápio.AdicionarItem(porção);
        }
        
        ConsoleDebugger.Imprimir(cardápio);

        Console.WriteLine("É HORA DO SHOW");

        AutomatizadorCompra automatizadorCompra = new();
        await automatizadorCompra.ExecutarCompra(cardápio, false);

        Console.WriteLine("E POR HOJE É SÓ");
    });

Console.ReadLine();