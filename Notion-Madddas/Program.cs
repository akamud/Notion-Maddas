using Notion.Client;
using NotionMaddas;
using Spectre.Console;

var client = NotionClientFactory.Create(new ClientOptions
{
    AuthToken = Environment.GetEnvironmentVariable("AuthToken")
});

var páginas = await client.Databases.QueryAsync(Environment.GetEnvironmentVariable("DatabaseId"),
    new DatabasesQueryParameters());

var porções = páginas.Results.SelectMany(x => x.Properties)
    .Where(x => x.Key == "Cardápio Letticia" || x.Key == "Cardapio Mud")
    .Select(x => x.Value).OfType<MultiSelectPropertyValue>()
    .SelectMany(value => value.MultiSelect)
    .Select(x => new Porção(x.Name))
    .ToList();

var cardápio = new Cardápio();
foreach (var porção in porções)
{
    cardápio.AdicionarItem(porção);
}

ConsoleDebugger.Imprimir(cardápio);

Console.ReadLine();
