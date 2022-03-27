using System.Text.RegularExpressions;

namespace NotionMaddas.Dominio;

public record Porção
{
    private const string padrãoPeso = @"\((\d+).*\)";
    private const string padrãoCompartilhado = "[Compartilhado]";

    public Porção(string nomeCompleto)
    {
        Nome = Regex.Replace(nomeCompleto, padrãoPeso, "").Replace( padrãoCompartilhado, "");
        var peso = Regex.Match(nomeCompleto, padrãoPeso).Groups.Values.FirstOrDefault(x => x.GetType() == typeof(Group));
        Peso = peso != null ? decimal.Parse(peso.Value) : null;
        Compartilhado = nomeCompleto.Contains(padrãoCompartilhado);
    }

    public string Nome { get; }
    public decimal? Peso { get; }
    public bool Compartilhado { get; }
}
