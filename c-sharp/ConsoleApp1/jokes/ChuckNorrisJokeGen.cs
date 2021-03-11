using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JokeGenerator.jokes
{
    public class ChuckNorisJokeGen : IJokeGen
    {

        private const string _baseUrl = "https://api.chucknorris.io";

        private readonly HttpClient _httpClient;

        public ChuckNorisJokeGen(HttpClient httpClient)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(_baseUrl);
        }

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

    }
}

﻿using Newtonsoft.Json;
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

    }
}
