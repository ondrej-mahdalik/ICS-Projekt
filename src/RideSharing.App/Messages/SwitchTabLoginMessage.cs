using RideSharing.Common.Enums;

namespace RideSharing.App.Messages;

public record SwitchTabLoginMessage(LoginViewIndex index) : IMessage;
