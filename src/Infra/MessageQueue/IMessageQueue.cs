namespace kendo_londrina.Infra.MessageQueue;

public interface IMessageQueue
{
    Task SendAsync(string message);
}
