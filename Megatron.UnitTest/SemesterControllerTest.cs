using Megatron.Controllers;
using Megatron.Services;
using Moq;
using NUnit.Framework;
using System;
using Megatron.ViewModels;

namespace UnitTest.Controllers
{
    [TestFixture]
    public class SemesterControllerTests
    {
        private MockRepository mockRepository;

        private Mock<ISemesterRepository> mockSemesterRepository;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockSemesterRepository = this.mockRepository.Create<ISemesterRepository>();
        }

        private SemesterController CreateSemesterController()
        {
            return new SemesterController(
                this.mockSemesterRepository.Object);
        }

        [Test]
        public void Index_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var semesterController = this.CreateSemesterController();

            // Act
            var result = semesterController.Index();

            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void GetSemesters_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var semesterController = this.CreateSemesterController();
            
            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var semesterController = this.CreateSemesterController();

           
            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }

       

        
    }
}
