using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace kendo_londrina.Infra.MessageQueue;

public class AmazonSQSService : IMessageQueue, IAsyncDisposable
{
    private readonly string _queueUrl;
    private readonly IAmazonSQS _sqsClient;

    public AmazonSQSService(IConfiguration config)
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

    public async Task SendAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or empty.", nameof(message));

        var request = new SendMessageRequest { QueueUrl = _queueUrl, MessageBody = message };

        try
        {
            var response = await _sqsClient.SendMessageAsync(request);
            Console.WriteLine($"Mensagem enviada com sucesso. ID: {response.MessageId}");
        }
        catch (AmazonSQSException ex)
        {
            Console.Error.WriteLine($"Erro ao enviar mensagem para SQS: {ex.Message}");
            throw; // repropaga, para permitir tratamento em nível superior
        }
    }

    public async ValueTask DisposeAsync()
    {
        // libera conexões HTTP subjacentes
        _sqsClient.Dispose();
        await Task.CompletedTask;
    }
}
