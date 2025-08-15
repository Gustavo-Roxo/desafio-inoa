using System;
using System.IO;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
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
                Console.WriteLine("Erro: As configurações de e-mail estão faltando no arquivo settings.json.");
                return;
            }

            if (settings?.ApiConfig == null)
            {
                Console.WriteLine("Erro: As configurações de api estão faltando no arquivo settings.json.");
                return;
            }

            Console.WriteLine("Configurações de e-mail lidas com sucesso.");

        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Erro ao ler o arquivo de configuração JSON: {ex.Message}");
            return;
        }

        Console.WriteLine($"Monitorando o ativo: {ativo}");
        Console.WriteLine($"Preço de venda de referência: {precoVenda}");
        Console.WriteLine($"Preço de compra de referência: {precoCompra}");

        while (true)
        {
            try
            {
                decimal currentPrice = await GetStockQuote(ativo, settings.ApiConfig.ApiKey);

                Console.WriteLine($"Cotação atual de {ativo}: {currentPrice:C}");

                // Lógica de envio de e-mail

                if (currentPrice > precoVenda)
                {
                    Console.WriteLine("Cotação subiu! Alerta de VENDA.");

                    // Chamar o método de envio de e-mail 

                }
                else if (currentPrice < precoCompra)
                {
                    Console.WriteLine("Cotação caiu! Alerta de COMPRA.");

                    // Chamar o método de envio de e-mail aqui

                }
                else
                {
                    Console.WriteLine("Preço dentro do intervalo, aguardando...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            }

            // Pausa o programa por 5 minutos antes da próxima verificação.
            await Task.Delay(300000);
        }
    }
    
        private static async Task<decimal> GetStockQuote(string symbol, string apiKey)
    {
        using (var client = new HttpClient())
        {
            string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={apiKey}";
            
            HttpResponseMessage response = await client.GetAsync(url);
            
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();



            // ADICIONAR A LÓGICA PARA ANALISAR O JSON E PEGAR O PREÇO.
            // Por enquanto, retornar um valor de teste para que o programa compile.
            
            return 22.50M;
        }
    }
}