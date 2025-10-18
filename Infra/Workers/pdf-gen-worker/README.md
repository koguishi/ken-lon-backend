# BackgroundService em .Net Core - parte do Projeto Kendo Londrina

Este projeto é um Worker que gera PDFs baseado nas mensagens de uma fila.
Os arquivos gerados são armazenados em um storage para posterior download.

## Serviços

* **Fila** — Amazon SQS
* **Storage** — Claudflare R2
* **Gerador de PDF** — iText

## Para rodar localmente

Criar contas e configurar os serviços Amazon SQS e Cloudflare R2
As informações abaixo devem ser preenchidas e inseridas no appsettings.json:
```json
{
  "AWS": {
    "Region": "",
    "AccessKey": "",
    "SecretKey": ""
  },
  "Sqs": {
    "QueueUrl": ""
  },  
  "CloudflareR2": {
    "AccountId" : "",
    "AccessKeyId" : "",
    "SecretAccessKey" : "",
    "FichaFinanceiraBucket": ""
  },
}
```
E, depois rodar a aplicação:
```bash
dotnet run
```
O Worker consome as mensagens do SQS, gera o PDF e armazena no R2.

## Testando
No console do Amazon SQS épossível enviar uma mensagem para a fila de forma manual.
No corpo da mensagem você pode colocar um conteúdo json de uma FichaFinanceiraDto:
Exemplo:
```json
{
  "JobId": "b2e7a7e4-8a4c-4e3a-9a3b-3d3d7a12f5b2",
  "Ano": 2025,
  "NomePessoa": "Outro Aluno",
  "Resumo": {
    "Total": 6000.01,
    "TotalLiquidado": 4500.00,
    "TotalEmAberto": 1500.00,
    "UltimaAtualizacao": "2025-10-11T09:15:00Z"
  },
  "Titulos": [
    {
      "Vencimento": "2025-01-10T00:00:00Z",
      "Valor": 1000.00,
      "DataLiquidacao": "2025-01-09T00:00:00Z"
    },
    {
      "Vencimento": "2025-02-10T00:00:00Z",
      "Valor": 1000.00,
      "DataLiquidacao": "2025-02-12T00:00:00Z"
    },
    {
      "Vencimento": "2025-03-10T00:00:00Z",
      "Valor": 1000.00,
      "DataLiquidacao": null
    },
    {
      "Vencimento": "2025-04-10T00:00:00Z",
      "Valor": 1000.00,
      "DataLiquidacao": null
    },
    {
      "Vencimento": "2025-05-10T00:00:00Z",
      "Valor": 1000.00,
      "DataLiquidacao": "2025-05-09T00:00:00Z"
    },
    {
      "Vencimento": "2025-06-10T00:00:00Z",
      "Valor": 1000.01,
      "DataLiquidacao": "2025-06-11T00:00:00Z"
    }
  ]
}
```
Isso adiciona um item na fila que será consumida pelo worker gerando o PDF.
O arquivo gerado pode ser visualizado no console da Cloudflare -> R2 object storage.

## Deploy

Este serviço roda como um Webjob de um AppServices do Azure (staging e produção).
Resumidamente:
  - gerar os arquivos publicáveis em uma estrutura de pasta específica
    (veja https://github.com/projectkudu/kudu/wiki/WebJobs)
  - compactar
  - publicar através do Kudu
Veja detalhes no script de deploy: .github\workflows\deploy_ken-lon-stg.yml

### Como configurar o segredo AZURE_CREDENTIALS

Você precisa criar um Service Principal para o GitHub Actions poder fazer login no Azure.
No terminal (ou Cloud Shell), execute:
```bash
az ad sp create-for-rbac \
  --name "github-actions-deploy" \
  --role contributor \
  --scopes /subscriptions/<subscription-id>/resourceGroups/<resource-group-name> \
  --sdk-auth
```
Isso retona um json como:
```json
{
  "clientId": "xxxx",
  "clientSecret": "xxxx",
  "subscriptionId": "xxxx",
  "tenantId": "xxxx",
  "activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
  "resourceManagerEndpointUrl": "https://management.azure.com/",
  "managementEndpointUrl": "https://management.core.windows.net/"
}
```
Copie todo o JSON e adicione no GitHub:
  - Vá em Settings → Secrets and variables → Actions → New secret
  - Nome: AZURE_CREDENTIALS
  - Valor: cole o JSON