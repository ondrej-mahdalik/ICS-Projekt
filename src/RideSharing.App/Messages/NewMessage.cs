using RideSharing.BL.Models;

namespace RideSharing.App.Messages;

public record NewMessage<T> : Message<T>
    where T : IModel
{
}
