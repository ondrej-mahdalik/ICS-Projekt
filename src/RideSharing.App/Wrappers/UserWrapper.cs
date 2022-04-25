using System;
using RideSharing.BL.Models;

namespace RideSharing.App.Wrappers;

public class UserWrapper : ModelWrapper<UserDetailModel>
{
    public UserWrapper(UserDetailModel model) : base(model)
    {
        throw new NotImplementedException();
    }

    private void InitializeCollectionProperties(UserDetailModel model)
    {
        throw new NotImplementedException();
    }

    public static implicit operator UserWrapper(UserDetailModel detailModel)
        => new(detailModel);

    public static implicit operator UserDetailModel(UserWrapper wrapper)
        => wrapper.ThisModel;
}
