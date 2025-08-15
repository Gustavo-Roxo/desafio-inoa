using System;
using System.IO;
using System.Text.Json;

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
        
        // Ler o arquivo de configuração
        string configFilePath = "settings.json";

        if (!File.Exists(configFilePath))
        {
            Console.WriteLine($"Erro: O arquivo de configuração '{configFilePath}' não foi encontrado.");
            return;
        }

        string jsonContent = File.ReadAllText(configFilePath);

        // Desserializar o JSON para o objeto AppSettings
        try
        {
            AppSettings settings = JsonSerializer.Deserialize<AppSettings>(jsonContent);

            if (settings?.EmailConfig == null)
            {
                Console.WriteLine("Erro: As configurações de e-mail estão faltando no arquivo appsettings.json.");
                return;
            }

            Console.WriteLine("Configurações de e-mail lidas com sucesso.");

        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Erro ao ler o arquivo de configuração JSON: {ex.Message}");
            return;
        }

    }
}