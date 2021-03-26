using Megatron.Controllers;
using Megatron.Services;
using Moq;
using NUnit.Framework;
using System;

namespace UnitTest.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IUserRepository> mockUserRepository;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
        }

        private UserController CreateUserController()
        {
            return new UserController(
                this.mockUserRepository.Object);
        }


        [Test]
        public void Lock_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userController = this.CreateUserController();
            string id = null;

            // Act
            var result = userController.Lock(
                id);

            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void UnLock_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userController = this.CreateUserController();
            string id = null;

            // Act
            var result = userController.UnLock(
                id);

            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }

        
    }
}
