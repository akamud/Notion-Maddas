using Spectre.Console;

namespace NotionMaddas;

public static class ConsoleDebugger
{
    public static void Imprimir(Cardápio cardápio)
    {
        var table = new Table();
        table.AddColumns(new TableColumn("Item").Footer("Total"),
            new TableColumn("Quantidade").Footer(cardápio.QuantidadeItens.ToString()));

        foreach (var (porção, quantidade) in cardápio.Itens.OrderBy(x => x.Porção.Nome))
        {
            table.AddRow($"{porção.Nome} {(porção.Peso == null ? "" : $"({porção.Peso}g)")}",
                quantidade.ToString());
        }

        AnsiConsole.Write(table);
    }
}
