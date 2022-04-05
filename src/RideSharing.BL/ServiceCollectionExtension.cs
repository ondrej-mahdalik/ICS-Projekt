using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RideSharing.BL.Facades;
using RideSharing.DAL.UnitOfWork;

namespace RideSharing.BL;

public static class ServiceCollectionExtension
{
    //public static IServiceCollection AddBLServices(this IServiceCollection services)
    //{
    //    services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
    //    services.AddSingleton<ReviewFacade>();
    //    services.AddSingleton<RideFacade>();
    //    services.AddSingleton<UserFacade>();
    //    services.AddSingleton<VehicleFacade>();
    //}
}
