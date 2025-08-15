using System;

class Program
{
    static void Main(string[] args)
    {
        // Verifica se o número de argumentos é o esperado
        if (args.Length != 3)
        {
            Console.WriteLine("Uso: inoa.exe <ativo> <preco_venda> <preco_compra>");
            Console.WriteLine("Exemplo: inoa.exe PETR4 22.67 22.59");
            return; // Encerra a aplicação
        }

        // Extrai os argumentos para variáveis
        string ativo = args[0];
        string precoVendaString = args[1];
        string precoCompraString = args[2];
        
        // Converte os preços para o tipo decimal
        decimal precoVenda;
        decimal precoCompra;

        if (!decimal.TryParse(precoVendaString, out precoVenda))
        {
            Console.WriteLine($"Erro: O preço de venda '{precoVendaString}' não é um número válido.");
            return;
        }

        if (!decimal.TryParse(precoCompraString, out precoCompra))
        {
            Console.WriteLine($"Erro: O preço de compra '{precoCompraString}' não é um número válido.");
            return;
        }

        Console.WriteLine($"Monitorando o ativo: {ativo}");
        Console.WriteLine($"Preço de venda de referência: {precoVenda}");
        Console.WriteLine($"Preço de compra de referência: {precoCompra}");

    }
}