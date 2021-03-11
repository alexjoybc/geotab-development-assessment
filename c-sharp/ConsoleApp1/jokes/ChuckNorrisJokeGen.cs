using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JokeGenerator.jokes
{
    /// <summary>
    /// Chuck Norris IJokeGen implemented using https://api.chucknorris.io api.
    /// </summary>
    public class ChuckNorrisJokeGen : IJokeGen
    {

        private const string _baseUrl = "https://api.chucknorris.io";

        private readonly HttpClient _httpClient;

        public ChuckNorrisJokeGen(HttpClient httpClient)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(_baseUrl);
        }

        /// <summary>
        /// Returns available categories, see https://api.chucknorris.io/jokes/categories
        /// </summary>
        /// <returns>A List of string</returns>
        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            try
            {
                string result = await _httpClient.GetStringAsync("/jokes/categories");
                return JsonConvert.DeserializeObject<List<string>>(result);
            } catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching categories from {_baseUrl}: {ex.Message}.");
                return new List<string>();
            } catch (JsonReaderException)
            {
                Console.WriteLine($"Error converting response to collections of categories.");
                return new List<string>();
            }
        }

        public async Task<string> GetRandomJokeAsync(string firstname, string lastname, string category)
        {

            string url = "jokes/random";
            if (category != null)
            {
                if (url.Contains('?'))
                    url += "&";
                else url += "?";
                url += "category=";
                url += category;
            }

            string reponse = await _httpClient.GetStringAsync(url);

            string joke = JsonConvert.DeserializeObject<dynamic>(reponse).value;

            if (firstname != null && lastname != null)
            {
                int index = joke.IndexOf("Chuck Norris");
                string firstPart = joke.Substring(0, index);
                string secondPart = joke.Substring(0 + index + "Chuck Norris".Length, joke.Length - (index + "Chuck Norris".Length));
                joke = firstPart.Trim() + " " + firstname + " " + lastname + secondPart;
            }

            return joke;
        }
    }
}
