namespace Shared.CustomTypes;

public interface IMessage
{
    Guid Id { get; }
    DateTime? CreatedDate { get; }
}