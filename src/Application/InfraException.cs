namespace kendo_londrina.Application;

public class InfraException(
    string? message,
    Exception? innerException = null
) : Exception(message, innerException)
{
}
