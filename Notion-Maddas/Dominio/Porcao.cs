using System.Text.RegularExpressions;

namespace NotionMaddas.Dominio;

public record Porção
{
    private const string padrãoPeso = @"\((\d+).*\)";
    private const string padrãoCompartilhado = "[Compartilhado]";

    public static Porção Parse(string nomeCompleto)
    {
        var peso = Regex.Match(nomeCompleto, padrãoPeso).Groups.Values
            .FirstOrDefault(x => x.GetType() == typeof(Group));
        return new Porção
        {
            Compartilhado = nomeCompleto.Contains(padrãoCompartilhado),
            Nome = Regex.Replace(nomeCompleto, padrãoPeso, "").Replace(padrãoCompartilhado, ""),
            Peso = peso != null ? int.Parse(peso.Value) : null
        };
    }

    public string Nome { get; private init; }
    public int? Peso { get; private init; }
    public bool Compartilhado { get; private init; }
}
