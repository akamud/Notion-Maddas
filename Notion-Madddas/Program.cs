// See https://aka.ms/new-console-template for more information

using Notion.Client;

var client = NotionClientFactory.Create(new ClientOptions
{
    AuthToken = Environment.GetEnvironmentVariable("AuthToken")
});

var pages = await client.Databases.QueryAsync(Environment.GetEnvironmentVariable("DatabaseId"), new DatabasesQueryParameters());
// var itensCardapio = pages.Properties.Where(x => x.Key == "Cardápio Letticia" || x.Key == "Cardapio Mud")
//     .Select(x => x.Value).OfType<MultiSelectProperty>().ToList();

Console.ReadLine();
