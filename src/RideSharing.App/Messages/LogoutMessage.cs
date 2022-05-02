using RideSharing.BL.Models;

namespace RideSharing.App.Messages;

public record LogoutMessage<T> : Message<T>
    where T : IModel
{
}
