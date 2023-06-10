using Microsoft.VisualBasic;
using System.Net;
using GPTNet.Models;
using GPTNet.Roles;
using Moq;
using Moq.Protected;

namespace GPTNet.Tests
{

    [TestFixture]
    public class GPTTests
    {
        [Test]
        public async Task Call_SuccessfulResponse_ReturnsGPTResponse()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"choices\": [{\"message\": {\"content\": \"Test response\"}}]}"),
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            var gpt = new GPT("test_api_key", "test_model_name", httpClient);
            var request = GetConversation();

            // Act
            var response = await gpt.Call(request);

            // Assert
            Assert.IsFalse(response.IsError);
            Assert.AreEqual("Test response", response.Response);
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public async Task Call_FailedResponse_ReturnsGPTResponse()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            var gpt = new GPT("test_api_key", "test_model_name", httpClient);
            var request = GetConversation();

            // Act
            var response = await gpt.Call(request);

            // Assert
            Assert.IsTrue(response.IsError);
            Assert.AreEqual(HttpStatusCode.BadRequest.ToString(), response.Error);
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        private static Conversation GetConversation()
        {
            return new Conversation(
                new Role(RoleType.System, new CustomRoleBehaviour("")),
                new Role(RoleType.Assistant, new CustomRoleBehaviour("")), false);
        }
    }
}
