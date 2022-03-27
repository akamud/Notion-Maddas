﻿using Microsoft.Playwright;
using NotionMaddas;
using NotionMaddas.Dominio;

var porções = await NotionParser.ObterPorções();

var cardápio = new Cardápio();
foreach (var porção in porções)
{
    cardápio.AdicionarItem(porção);
}

ConsoleDebugger.Imprimir(cardápio);

using var playwright = await Playwright.CreateAsync();
await using var browser =
    await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 200 });
var context = await browser.NewContextAsync(new BrowserNewContextOptions { BaseURL = "https://maddas.com.br/" });
var page = await context.NewPageAsync();

foreach (var item in cardápio.Itens)
{
    await page.GotoAsync($"busca?palavra={item.Porção.Nome}");
    await page.ClickAsync("text=Adicionar ao carrinho");
    await page.ClickAsync("text=Continuar comprando");
}

Console.ReadLine();
