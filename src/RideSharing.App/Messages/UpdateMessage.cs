using RideSharing.BL.Models;

namespace RideSharing.App.Messages;

public record UpdateMessage<T> : Message<T>
    where T : IModel
{
}
