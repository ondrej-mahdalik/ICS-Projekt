using RideSharing.BL.Models;

namespace RideSharing.App.Messages;

public record DetailMessage<T> : Message<T>
    where T : IModel
{
}
