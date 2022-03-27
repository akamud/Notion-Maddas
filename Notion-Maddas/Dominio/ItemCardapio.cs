namespace NotionMaddas.Dominio;

public record ItemCardapio(Porção Porção, int Quantidade = 1)
{
    public int Quantidade { get; private set; } = Quantidade;

    public void Incrementar()
    {
        if (!Porção.Compartilhado)
            Quantidade++;
    }
}
