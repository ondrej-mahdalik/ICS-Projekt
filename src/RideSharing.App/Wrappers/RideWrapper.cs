using System;
using RideSharing.BL.Models;

namespace RideSharing.App.Wrappers;

public class RideWrapper : ModelWrapper<RideDetailModel>
{
    public RideWrapper(RideDetailModel model) : base(model)
    {
        throw new NotImplementedException();
    }

    private void InitializeCollectionProperties(RideDetailModel model)
    {
        throw new NotImplementedException();
    }

    public static implicit operator RideWrapper(RideDetailModel detailModel)
        => new(detailModel);

    public static implicit operator RideDetailModel(RideWrapper wrapper)
        => wrapper.ThisModel;
}
