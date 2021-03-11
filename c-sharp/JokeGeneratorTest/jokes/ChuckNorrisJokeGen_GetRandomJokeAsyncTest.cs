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

        private ChuckNorrisJokeGen sut;

        [DataRow("{\"value\":\"Chuck Norris trims his fingernails with a chainsaw.\"}", null, null, null, "Chuck Norris trims his fingernails with a chainsaw.")]
        [DataRow("{\"value\":\"Chuck Norris trims his fingernails with a chainsaw.\"}", "Bob", "Ross", null, "Bob Ross trims his fingernails with a chainsaw.")]
        [DataTestMethod]
        public void positiveTests(string joke, string firstName, string lastName, string category, string expected)
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

            sut = new ChuckNorrisJokeGen(httpClient);

            var actual = sut.GetRandomJokeAsync(firstName, lastName, category).Result;

            Assert.AreEqual(expected, actual);

        }




    }
}
