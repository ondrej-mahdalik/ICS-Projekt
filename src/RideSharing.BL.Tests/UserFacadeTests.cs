using System.Threading.Tasks;
using Xunit;
using RideSharing.BL.Models;
using RideSharing.BL.Facades;
using Xunit.Abstractions;

namespace RideSharing.BL.Tests
{
    public sealed class UserFacadeTests : CRUDFacadeTestsBase
    {
        private readonly UserFacade _userFacadeSUT;

        public UserFacadeTests(ITestOutputHelper output) : base(output)
        {
            _userFacadeSUT = new UserFacade(UnitOfWorkFactory, Mapper);
        }
        [Fact]
        public async Task Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new UserDetailModel(
                Name: @"Jan",
                Surname: @"Václavík",
                Phone: @"746652914"
            );

            var _ = await _userFacadeSUT.SaveAsync(model);
        }
    }
}
