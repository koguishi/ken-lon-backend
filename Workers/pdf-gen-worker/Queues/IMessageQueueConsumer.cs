namespace pdf_gen_worker.Queues;
public interface IMessageQueueConsumer
{
    Task ConsumeAsync(
        string queueName,
        Func<string, Task> onMessageAsync,
        CancellationToken cancellationToken
    );
}
