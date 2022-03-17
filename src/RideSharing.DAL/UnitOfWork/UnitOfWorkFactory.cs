using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RideSharing.DAL.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IDbContextFactory<CookBookDbContext> _dbContextFactory;

        public UnitOfWorkFactory(IDbContextFactory<CookBookDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public IUnitOfWork Create() => new UnitOfWork(_dbContextFactory.CreateDbContext());
    }
}
