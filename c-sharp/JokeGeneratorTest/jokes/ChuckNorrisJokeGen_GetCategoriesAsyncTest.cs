using JokeGenerator.jokes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace JokeGeneratorTest.jokes
{
    [TestClass]
    public class ChuckNorrisJokeGen_GetCategoriesAsyncTest
    {

        private const string _url = "http://example.com";

        private ChuckNorrisJokeGen sut;

        [TestMethod]
        public async Task With200ShouldReturnAListOfCategories()
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
                   Content = new StringContent(@"['a','b','c','d']"),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri(_url);

            sut = new ChuckNorrisJokeGen(httpClient);

            IEnumerable<string> actual = await sut.GetCategoriesAsync();

            Assert.AreEqual(4, actual.Count());

            Assert.AreEqual("a", actual.ElementAt(0));
            Assert.AreEqual("b", actual.ElementAt(1));
            Assert.AreEqual("c", actual.ElementAt(2));
            Assert.AreEqual("d", actual.ElementAt(3));

            var expectedUri = new Uri($"{_url}/jokes/categories");

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
        public async Task With400ShouldReturnAnEmptyListOfCategories()
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
                   StatusCode = HttpStatusCode.BadRequest,
                   Content = new StringContent(@"['a','b','c','d']"),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri(_url);

            sut = new ChuckNorrisJokeGen(httpClient);

            IEnumerable<string> actual = await sut.GetCategoriesAsync();

            Assert.AreEqual(0, actual.Count());

            var expectedUri = new Uri($"{_url}/jokes/categories");

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
        public async Task WithInvalidResponseFromChuckShouldReturnAnEmptyListOfCategories()
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
                   Content = new StringContent(@"{\'response\': \'nope\'}"),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri(_url);

            sut = new ChuckNorrisJokeGen(httpClient);

            IEnumerable<string> actual = await sut.GetCategoriesAsync();

            Assert.AreEqual(0, actual.Count());

            var expectedUri = new Uri($"{_url}/jokes/categories");

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
