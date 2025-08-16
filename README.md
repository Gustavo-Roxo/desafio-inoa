# desafio-inoa  🚀 Como Usar
1. Configuração Inicial
Primeiro, clone este repositório e navegue até a pasta do projeto:

Bash

git clone <URL_DO_SEU_REPOSITORIO>
cd <nome_da_pasta_do_projeto>
Em seguida, abra o arquivo settings.json e preencha-o com suas informações e credenciais:

JSON

{
  "EmailConfig": {
    "ToEmail": "seu_email_de_destino@exemplo.com",
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "seu_email_de_envio@gmail.com",
    "SmtpPassword": "sua_senha_de_aplicativo_aqui"
  },
  "ApiConfig": {
    "ApiKey": "SUA_CHAVE_DA_ALPHA_VANTAGE_AQUI"
  }
}
2. Executando a Aplicação
Use o comando dotnet run no terminal, passando o ativo e os preços de referência como argumentos.

Formato do Comando:

Bash

dotnet run <simbolo_do_ativo> <preco_de_venda> <preco_de_compra>
Exemplo:
Para monitorar a PETR4, enviando um alerta de venda se o preço subir acima de R$ 22,67 e um alerta de compra se cair abaixo de R$ 22,59:

Bash

dotnet run PETR4.SA 22.67 22.59
Observação: O programa rodará em um loop infinito, verificando o preço a cada 5 minutos. Pressione Ctrl + C para encerrá-lo.

