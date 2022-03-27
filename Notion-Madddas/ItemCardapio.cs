namespace NotionMaddas;

public record ItemCardapio
{
    public ItemCardapio(Porção porção, int quantidade = 1)
    {
        Porção = porção;
        Quantidade = quantidade;
    }

    public Porção Porção { get; }
    public int Quantidade { get; private set; }

    public void Incrementar()
    {
        if (!Porção.Compartilhado)
            Quantidade++;
    }
}