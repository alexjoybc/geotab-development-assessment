using JokeGenerator.names;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JokeGeneratorTest
{
    [TestClass]
    public class NamesPrivservNameGenTest
    {
        private const string _url = "http://example.com";

        private NamesPrivservNameGen sut;

        
        [TestMethod]
        public async Task With200ResponseShouldReturnFirstNameLastName()
        {

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("{\"name\":\"Scott\",\"surname\":\"Bon\",\"gender\":\"male\",\"region\":\"Romania\"}"),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri(_url);

            sut = new NamesPrivservNameGen(httpClient);

            var result = await sut.GetRandomNameAsync();

            Assert.AreEqual("Scott", result.Item1);
            Assert.AreEqual("Bon", result.Item2);

        }

        [TestMethod]
        public async Task With400ResponseShouldReturnNull()
        {

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.BadRequest
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri(_url);

            sut = new NamesPrivservNameGen(httpClient);

            var result = await sut.GetRandomNameAsync();

            var expectedUri = new Uri($"{_url}/api");

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1), // we expected a single external request
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get  // we expected a GET request
                  && req.RequestUri == expectedUri // to this uri
               ),
               ItExpr.IsAny<CancellationToken>()
            );

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task With200ResponseAndInvalidJsonShouldReturnNull()
        {

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("test"),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri(_url);

            sut = new NamesPrivservNameGen(httpClient);

            var result = await sut.GetRandomNameAsync();

            Assert.IsNull(result);

            var expectedUri = new Uri($"{_url}/api");

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1), // we expected a single external request
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get  // we expected a GET request
                  && req.RequestUri == expectedUri // to this uri
               ),
               ItExpr.IsAny<CancellationToken>()
            );

        }

        [TestMethod]
        public async Task With200ResponseAndNoNamInJsonShouldReturnNull()
        {

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(@"{'gender':'male'}"),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri(_url);

            sut = new NamesPrivservNameGen(httpClient);

            var result = await sut.GetRandomNameAsync();

            Assert.IsNull(result);

            var expectedUri = new Uri($"{_url}/api");

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1), // we expected a single external request
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get  // we expected a GET request
                  && req.RequestUri == expectedUri // to this uri
               ),
               ItExpr.IsAny<CancellationToken>()
            );

        }

    }
}
