using NotionMaddas;
using NotionMaddas.Dominio;

var porções = await NotionParser.ObterPorções();

var cardápio = new Cardápio();
foreach (var porção in porções)
{
    cardápio.AdicionarItem(porção);
}

ConsoleDebugger.Imprimir(cardápio);

Console.WriteLine("É HORA DO SHOW");

AutomatizadorCompra automatizadorCompra = new();
await automatizadorCompra.ExecutarCompra(cardápio, false);

Console.WriteLine("E POR HOJE É SÓ");

Console.ReadLine();
