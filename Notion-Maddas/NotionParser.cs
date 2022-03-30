using Notion.Client;
using NotionMaddas.Dominio;

namespace NotionMaddas;

public static class NotionParser
{
    public static async Task<IReadOnlyCollection<Porção>> ObterPorções(string databaseId)
    {
        var client = NotionClientFactory.Create(new ClientOptions
        {
            AuthToken = Environment.GetEnvironmentVariable("NotionAuthToken")
        });

        var páginas = await client.Databases.QueryAsync(databaseId,
            new DatabasesQueryParameters());

        return páginas.Results.SelectMany(x => x.Properties)
            .Where(x => x.Key is "Cardápio Letticia" or "Cardapio Mud")
            .Select(x => x.Value).OfType<MultiSelectPropertyValue>()
            .SelectMany(value => value.MultiSelect)
            .Select(x => Porção.Parse(x.Name))
            .ToList();
    }
}
