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

namespace JokeGeneratorTest.jokes
{


    [TestClass]
    public class ChuckNorrisJokeGen_GetRandomJokeAsyncTest
    {

        private const string _url = "http://example.com";

        private ChuckNorrisJokeGen sut;

        [DataRow("{\"value\":\"Chuck Norris trims his fingernails with a chainsaw.\"}", null, null, "Chuck Norris trims his fingernails with a chainsaw.")]
        [DataRow("{\"value\":\"Chuck Norris trims his fingernails with a chainsaw.\"}", "Bob", "Ross", "Bob Ross trims his fingernails with a chainsaw.")]
        [DataRow("{\"value\":\"Chuck Norris trims his fingernails with a chainsaw.\"}", "Bob", "Ross", "Bob Ross trims his fingernails with a chainsaw.")]
        [DataRow("{\"value\":\"Chuck Norris trims his fingernails with a chainsaw. says Chuck Norris\"}", "Bob", "Ross", "Bob Ross trims his fingernails with a chainsaw. says Bob Ross")]
        [DataRow("{\"value\":\"Chuck Norris trims his fingernails with a chainsaw. says Chuck\"}", "Bob", "Ross", "Bob Ross trims his fingernails with a chainsaw. says Bob")]
        [DataRow("{\"value\":\"CHUCK Norris trims his fingernails with a chainsaw. says Chuck\"}", "Bob", "Ross", "Bob Ross trims his fingernails with a chainsaw. says Bob")]
        [DataRow("{\"value\":\"CHUCKchuckChUcK Norris trims his fingernails with a chainsaw. says Chuck\"}", "Bob", "Ross", "BobBobBob Ross trims his fingernails with a chainsaw. says Bob")]
        [DataRow("{\"value\":\"NORRIS trims his fingernails with a chainsaw. says Chuck\"}", "Bob", "Ross", "Ross trims his fingernails with a chainsaw. says Bob")]
        [DataRow("{\"value\":\"unknown trims his fingernails with a chainsaw.\"}", "Bob", "Ross", "unknown trims his fingernails with a chainsaw.")]
        [DataTestMethod]
        public void testSwappNameJokeGenerator(string joke, string firstName, string lastName, string expected)
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
                   Content = new StringContent(joke)
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri(_url);

            sut = new ChuckNorrisJokeGen(httpClient);

            var actual = sut.GetRandomJokeAsync(firstName, lastName, null).Result;

            Assert.AreEqual(expected, actual);

            var expectedUri = new Uri($"{_url}/jokes/random");

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  && req.RequestUri == expectedUri
               ),
               ItExpr.IsAny<CancellationToken>()
            );

        }

        [TestMethod]
        public void withCategoryShouldReturnAJoke()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("{\"value\":\"Chuck Norris trims his fingernails with a chainsaw.\"}")
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri(_url);

            sut = new ChuckNorrisJokeGen(httpClient);

            var actual = sut.GetRandomJokeAsync(null, null, "animal").Result;

            Assert.AreEqual("Chuck Norris trims his fingernails with a chainsaw.", actual);

            var expectedUri = new Uri($"{_url}/jokes/random?category=animal");

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  && req.RequestUri == expectedUri
               ),
               ItExpr.IsAny<CancellationToken>()
            );

        }


        [TestMethod]
        public void withInvalidJsonShouldReturnNull()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("{\"valuere\":\"Chuck Norris trims his fingernails with a chainsaw.\"}")
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri(_url);

            sut = new ChuckNorrisJokeGen(httpClient);

            var actual = sut.GetRandomJokeAsync(null, null, null).Result;

            Assert.IsNull(actual);

            var expectedUri = new Uri($"{_url}/jokes/random");

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  && req.RequestUri == expectedUri
               ),
               ItExpr.IsAny<CancellationToken>()
            );

        }




    }
}
