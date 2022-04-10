using Microsoft.EntityFrameworkCore;

namespace RideSharing.DAL.UnitOfWork;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IDbContextFactory<RideSharingDbContext> _dbContextFactory;

    public UnitOfWorkFactory(IDbContextFactory<RideSharingDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public IUnitOfWork Create()
    {
        return new UnitOfWork(_dbContextFactory.CreateDbContext());
    }
}
