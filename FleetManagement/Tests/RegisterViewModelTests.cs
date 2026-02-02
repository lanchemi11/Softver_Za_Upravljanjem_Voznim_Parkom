using Xunit;
using FleetManagement.ViewModels;
using FleetManagement.Data;
using FleetManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FleetManagement.Tests
{
    public class RegisterViewModelTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public void Register_ShouldFail_WhenRoleIsEmpty()
        {
            var context = GetInMemoryContext();
            var vm = new RegisterViewModel(context)
            {
                Username = "testuser",
                Password = "12345",
                SelectedRole = ""
            };

            vm.RegisterCommand.Execute(null);

            Assert.False(context.Users.Any());
        }

        [Fact]
        public void Register_ShouldFail_WhenUsernameAlreadyExists()
        {
            var context = GetInMemoryContext();
            context.Users.Add(new Administrator { Username = "testuser", Lozinka = "123", Rola = "Administrator" });
            context.SaveChanges();

            var vm = new RegisterViewModel(context)
            {
                Username = "testuser",
                Password = "12345",
                SelectedRole = "Administrator"
            };

            vm.RegisterCommand.Execute(null);

            Assert.Equal(1, context.Users.Count());
        }

        [Fact]
        public void Register_ShouldSucceed_WhenValidDataProvided()
        {
            var context = GetInMemoryContext();
            var vm = new RegisterViewModel(context)
            {
                Username = "newuser",
                Password = "12345",
                SelectedRole = "Vozac",
                BrojLicence = "LIC-001"
            };

            vm.RegisterCommand.Execute(null);

            var user = context.Users.FirstOrDefault(u => u.Username == "newuser");
            Assert.NotNull(user);
            Assert.Equal("Vozac", user.Rola);
        }
    }
}