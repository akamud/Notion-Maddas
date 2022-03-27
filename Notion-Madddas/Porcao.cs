using System.Text.RegularExpressions;

namespace NotionMaddas;

public record Porção
{
    private const string padrãoPeso = @"\((\d+).*\)";

    public Porção(string nome)
    {
        Nome = Regex.Replace(nome, padrãoPeso, "");
        var peso = Regex.Match(nome, padrãoPeso).Groups.Values.FirstOrDefault(x => x.GetType() == typeof(Group));
        Peso = peso == null ? 100 : decimal.Parse(peso.Value);
    }

    public string Nome { get; }
    public decimal Peso { get; }
}
