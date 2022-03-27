namespace NotionMaddas;

public record Cardápio
{
    public IList<ItemCardapio> Itens { get; } = new List<ItemCardapio>();
    
    public void AdicionarItem(ItemCardapio item)
    {
        var itemExistente = Itens.FirstOrDefault(item);
    }
}