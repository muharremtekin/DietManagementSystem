using System.Runtime.Serialization;

namespace DietManagementSystem.Application.Exceptions;

[Serializable]
public class NotFoundException : Exception
{
    private string v;
    private string userName;

    public NotFoundException()
    {
    }

    public NotFoundException(string? message) : base(message)
    {
    }

    public NotFoundException(string v, string userName)
    {
        this.v = v;
        this.userName = userName;
    }

    public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}