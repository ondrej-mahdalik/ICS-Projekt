using RideSharing.Common.Enums;

namespace RideSharing.App.Messages;

public record SwitchTabMessage(ViewIndex index) : IMessage;
