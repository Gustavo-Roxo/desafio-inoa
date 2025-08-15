using System;
using System.IO;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Uso: inoa.exe <ativo> <preco_venda> <preco_compra>");
            Console.WriteLine("Exemplo: inoa.exe PETR4 22.67 22.59");
            return;
        }

        string ativo = args[0];
        string precoVendaString = args[1];
        string precoCompraString = args[2];

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

        string configFilePath = "settings.json";

        if (!File.Exists(configFilePath))
        {
            Console.WriteLine($"Erro: O arquivo de configuração '{configFilePath}' não foi encontrado.");
            return;
        }

        AppSettings? settings = null;

        try
        {
            string jsonContent = File.ReadAllText(configFilePath);
            settings = JsonSerializer.Deserialize<AppSettings>(jsonContent);

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
                if (settings?.ApiConfig?.ApiKey == null)
                {
                    Console.WriteLine("Erro: A chave da API não foi encontrada nas configurações.");
                    return;
                }

                decimal currentPrice = await GetStockQuote(ativo, settings.ApiConfig.ApiKey);

                Console.WriteLine($"Cotação atual de {ativo}: {currentPrice:C}");

                if (currentPrice > precoVenda)
                {
                    Console.WriteLine("Cotação subiu! Alerta de VENDA.");
                    string subject = $"Alerta de VENDA: {ativo} em alta!";
                    string body = $"A cotação de **{ativo}** subiu para **{currentPrice:C}**, ultrapassando o valor de referência de VENDA de {precoVenda:C}.";
                    await SendEmailAlert(settings.EmailConfig?.ToEmail, subject, body, settings.EmailConfig);
                }
                else if (currentPrice < precoCompra)
                {
                    Console.WriteLine("Cotação caiu! Alerta de COMPRA.");
                    string subject = $"Alerta de COMPRA: {ativo} em baixa!";
                    string body = $"A cotação de **{ativo}** caiu para **{currentPrice:C}**, atingindo o valor de referência de COMPRA de {precoCompra:C}.";
                    await SendEmailAlert(settings.EmailConfig?.ToEmail, subject, body, settings.EmailConfig);
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
            AlphaVantageApiResponse? apiResponse = JsonSerializer.Deserialize<AlphaVantageApiResponse>(responseBody);
            
            if (apiResponse?.GlobalQuote == null || string.IsNullOrEmpty(apiResponse.GlobalQuote.PriceString))
            {
                throw new Exception($"Não foi possível obter a cotação para o símbolo '{symbol}'. Verifique se o símbolo está correto.");
            }

            if (decimal.TryParse(apiResponse.GlobalQuote.PriceString, out decimal price))
            {
                return price;
            }
            else
            {
                throw new Exception($"Erro ao converter o preço '{apiResponse.GlobalQuote.PriceString}' para número.");
            }
        }
    }
    
    private static async Task SendEmailAlert(string? toEmail, string subject, string body, EmailConfig? config)
    {
        try
        {
            if (config?.SmtpServer == null || config.SmtpUsername == null || config.SmtpPassword == null)
            {
                Console.WriteLine("Erro: Configurações de e-mail incompletas.");
                return;
            }

            var smtpClient = new SmtpClient(config.SmtpServer, config.SmtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(config.SmtpUsername, config.SmtpPassword),
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(config.SmtpUsername),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            
            if (toEmail != null)
            {
                mailMessage.To.Add(toEmail);
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine($"Alerta de e-mail enviado com sucesso para {toEmail}.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar o e-mail: {ex.Message}");
        }
    }
}