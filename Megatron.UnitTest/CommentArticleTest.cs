using System;
using Megatron.Models;
using Moq;
using NUnit.Framework;

namespace UnitTest.Models
{
    [TestFixture]
    public class CommentArticleTests
    {
        private MockRepository mockRepository;



        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private CommentArticle CreateCommentArticle()
        {
            return new CommentArticle();
        }

        [Test]
        public void TestMethod1()
        {
            // Arrange
            var commentArticle = this.CreateCommentArticle();

            // Act

                
            // Assert
            Assert.Pass();
            this.mockRepository.VerifyAll();
        }
    }
}