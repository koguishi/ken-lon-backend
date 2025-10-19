using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace pdf_gen_worker.Queues;

public class AmazonSqsService : IMessageQueueConsumer, IAsyncDisposable
{
    private readonly string _queueUrl;
    private readonly IAmazonSQS _sqsClient;

    public AmazonSqsService(IConfiguration config)
    {
        _queueUrl = config["SQS:QueueUrl"]
            ?? throw new InvalidOperationException("SQS QueueUrl is not configured.");

        var regionName = config["AWS:Region"]
            ?? throw new InvalidOperationException("AWS Region is not configured.");

        var accessKey = config["AWS:AccessKey"]
            ?? throw new InvalidOperationException("AWS Acess Key is not configured.");

        var secretKey = config["AWS:SecretKey"]
            ?? throw new InvalidOperationException("AWS Secret Key is not configured.");

        // Config AWS
        var sqsConfig = new AmazonSQSConfig { RegionEndpoint = RegionEndpoint.GetBySystemName(regionName) };

        // Usa a factory do SDK, permitindo reuso e testes mais fáceis
        _sqsClient = new AmazonSQSClient(accessKey, secretKey, sqsConfig);
    }

    public async Task ConsumeAsync(
        string queueName,
        Func<string, Task> onMessageAsync,
        CancellationToken cancellationToken)
    {
        var request = new ReceiveMessageRequest
        {
            QueueUrl = _queueUrl,
            MaxNumberOfMessages = 5,
            WaitTimeSeconds = 10 // long polling
        };

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var response = await _sqsClient.ReceiveMessageAsync(request, cancellationToken);

                if (response.Messages == null || response.Messages.Count == 0)
                {
                    // Nenhuma mensagem recebida neste ciclo — apenas continua o loop
                    Console.WriteLine("Waiting messages ...");
                    continue;
                }

                foreach (var message in response.Messages)
                {
                    try
                    {
                        Console.WriteLine($"Recebida: {message.Body}");

                        // processa a mensagem
                        await onMessageAsync(message.Body);

                        // remove da fila após sucesso
                        await _sqsClient.DeleteMessageAsync(_queueUrl, message.ReceiptHandle, cancellationToken);
                        Console.WriteLine($"{DateTime.Now.ToString("dd/MM/yy - HH:mm:ss")} - Mensagem removida: {message.MessageId}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar mensagem {message.MessageId}: {ex.Message}");
                        // Aqui você pode optar por não deletar, para que a mensagem volte à fila após o VisibilityTimeout
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Consumo cancelado.");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao receber mensagens: {ex.Message}");
                // Espera um pouco antes de tentar novamente para evitar loop frenético
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }

        Console.WriteLine("Consumo encerrado.");
    }

    public async ValueTask DisposeAsync()
    {
        // libera conexões HTTP subjacentes
        _sqsClient.Dispose();
        await Task.CompletedTask;
    }
}
