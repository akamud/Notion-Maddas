namespace NotionMaddas;

public record Cardápio
{
    public IList<ItemCardapio> Itens { get; } = new List<ItemCardapio>();

    public void AdicionarItem(Porção porção)
    {
        var itemExistente = Itens.FirstOrDefault(x => x.Porção == porção);
        if (itemExistente == null)
        {
            Itens.Add(new ItemCardapio(porção));
        }
        else
        {
            itemExistente.Incrementar();
        }
    }

    public int QuantidadeItens => Itens.Sum(x => x.Quantidade);
}
