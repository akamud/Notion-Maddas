using Spectre.Console;

namespace NotionMaddas;

public static class ConsoleDebugger
{
    public static void Imprimir(Cardápio cardápio)
    {
        var table = new Table();
        table.AddColumns("Item", "Quantidade");

        foreach (var item in cardápio.Itens.OrderBy(x => x.Porção.Nome))
        {
            table.AddRow($"{item.Porção.Nome} {(item.Porção.Peso == null ? "" : $"({item.Porção.Peso}g)")}",
                item.Quantidade.ToString());
        }

        table.AddRow("Total", cardápio.QuantidadeItens.ToString());

        AnsiConsole.Write(table);
    }
}
