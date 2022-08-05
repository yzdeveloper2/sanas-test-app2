using PrismApp.Services;

namespace PrismApp.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task UserServiceLoadsUsers()
        {
            // Given
            const string url = "https://sanas-test-api.azurewebsites.net/api/test/get-users";

            var service = new UserService(url);

            // When
            var users = await service.GetUsers();

            // Then
            var us = users.ToList();
            Assert.Equal(40, us.Count);
            Assert.DoesNotContain(us, u => u.Id == -1);
            Assert.DoesNotContain(us, u => string.IsNullOrEmpty(u.FirstName));
            Assert.DoesNotContain(us, u => string.IsNullOrEmpty(u.LastName));
            Assert.DoesNotContain(us, u => string.IsNullOrEmpty(u.Title));
            Assert.Equal(2, us.Where(u => u.SupervisorId == null).Count());
        }
    }
}