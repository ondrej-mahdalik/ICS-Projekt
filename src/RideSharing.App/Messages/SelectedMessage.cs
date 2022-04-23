using RideSharing.BL.Models;

namespace RideSharing.App.Messages;

public record SelectedMessage<T> : Message<T>
    where T : IModel
{
}
