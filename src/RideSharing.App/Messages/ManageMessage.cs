using RideSharing.BL.Models;

namespace RideSharing.App.Messages;

public record ManageMessage<T> : Message<T>
    where T : IModel
{
}
