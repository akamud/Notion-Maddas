using Notion.Client;

namespace NotionMaddas.Dominio;

public static class NotionParser
{
    public static async Task<IReadOnlyCollection<Porção>> ObterPorções()
    {
        var client = NotionClientFactory.Create(new ClientOptions
        {
            AuthToken = Environment.GetEnvironmentVariable("AuthToken")
        });

        var páginas = await client.Databases.QueryAsync(Environment.GetEnvironmentVariable("DatabaseId"),
            new DatabasesQueryParameters());

        return páginas.Results.SelectMany(x => x.Properties)
            .Where(x => x.Key is "Cardápio Letticia" or "Cardapio Mud")
            .Select(x => x.Value).OfType<MultiSelectPropertyValue>()
            .SelectMany(value => value.MultiSelect)
            .Select(x => new Porção(x.Name))
            .ToList();
    }
}
