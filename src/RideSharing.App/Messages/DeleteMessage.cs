using RideSharing.BL.Models;

namespace RideSharing.App.Messages;

public record DeleteMessage<T> : Message<T>
    where T : IModel
{
}
