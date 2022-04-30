using RideSharing.BL.Models;

namespace RideSharing.App.Messages;

public record LoginMessage<T> : Message<T>
    where T : IModel
{
}
