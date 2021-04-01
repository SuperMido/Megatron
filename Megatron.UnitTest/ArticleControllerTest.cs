using Megatron.Controllers;
using Megatron.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace UnitTest.Controllers
{
    [TestFixture]
    public class ArticleControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IArticleRepository> mockArticleRepository;
        private Mock<ICommentRepository> mockCommentRepository;
        private Mock<IUserRepository> mockUserRepository;
        private Mock<IDocumentRepository> mockDocumentRepository;
        private Mock<ISemesterRepository> mockSemesterRepository;


        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockArticleRepository = this.mockRepository.Create<IArticleRepository>();
            this.mockCommentRepository = this.mockRepository.Create<ICommentRepository>();
            this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
            this.mockDocumentRepository = this.mockRepository.Create<IDocumentRepository>();
            this.mockSemesterRepository = this.mockRepository.Create<ISemesterRepository>();
        }

        private ArticleController CreateArticleController()
        {
            return new ArticleController(
                this.mockArticleRepository.Object,
                this.mockCommentRepository.Object,
                this.mockUserRepository.Object,
                this.mockDocumentRepository.Object,
                this.mockSemesterRepository.Object);
        }

        [Test]
        public void Index_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var articleController = this.CreateArticleController();

            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void ListArticles_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var articleController = this.CreateArticleController();
            int id = 0;
            

            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void ListArticlesApproved_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var articleController = this.CreateArticleController();

        
            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void Details_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var articleController = this.CreateArticleController();
            int id = 0;
            

            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void GetListArticles_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var articleController = this.CreateArticleController();
            int id = 0;

            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void UpdateArticleStatus_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var articleController = this.CreateArticleController();
            int articleId = 0;
            bool status = false;
            string statusMessage = null;
            
            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task SendMessage_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var articleController = this.CreateArticleController();
            int articleId = 0;
            string message = null;

            // Act
            var result = await articleController.SendMessage(
                articleId,
                message);

            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }

      
    }
}
