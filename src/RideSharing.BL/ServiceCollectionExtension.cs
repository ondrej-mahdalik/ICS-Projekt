using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using RideSharing.BL.Facades;
using RideSharing.DAL;
using RideSharing.DAL.UnitOfWork;

namespace RideSharing.BL;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBLServices(this IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
        services.AddSingleton<ReviewFacade>();
        services.AddSingleton<RideFacade>();
        services.AddSingleton<UserFacade>();
        services.AddSingleton<VehicleFacade>();
        services.AddSingleton<ReservationFacade>();

        services.AddAutoMapper((serviceProvider, cfg) =>
        {
            cfg.AddCollectionMappers();

            var dbContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<RideSharingDbContext>>();
            using var dbContext = dbContextFactory.CreateDbContext();
            cfg.UseEntityFrameworkCoreModel<RideSharingDbContext>(dbContext.Model);

        }, typeof(BusinessLogic).Assembly);

        return services;
    }
}
