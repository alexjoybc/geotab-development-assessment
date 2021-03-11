using Microsoft.CSharp.RuntimeBinder;
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

        
        private const string _chuck = "chuck";
        private const string _norris = "norris";

        private readonly HttpClient _httpClient;

        public ChuckNorrisJokeGen(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Returns available categories, see https://api.chucknorris.io/jokes/categories
        /// </summary>
        /// <returns>A List of string</returns>
        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {

            List<string> categories = new List<string>();

            try
            {
                var result = await _httpClient.GetStringAsync("/jokes/categories");
                categories.AddRange(JsonConvert.DeserializeObject<List<string>>(result));
            } catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching categories from {_httpClient.BaseAddress}: {ex.Message}.");
            } catch (JsonReaderException)
            {
                Console.WriteLine($"Error converting response to collections of categories.");
            }

            return categories;

        }

        /// <summary>
        /// Returns a chuck norris joke, see https://api.chucknorris.io/jokes/random
        /// 
        /// Usage notes:
        /// 
        /// if you provide a firstname and a lastname then it will swap the chuck with this new hero
        /// if you provide a category it will filter jokes
        /// you can use the category api to retrieve the list of available api. invalid categories will mostly return a 404 and cause a null value.
        /// 
        /// </summary>
        /// <param name="firstname">A new hero first name</param>
        /// <param name="lastname">A new hero last name</param>
        /// <param name="category">A category filter</param>
        /// <returns></returns>
        public async Task<string> GetRandomJokeAsync(string firstname, string lastname, string category)
        {

            string url = string.IsNullOrWhiteSpace(category) ? "jokes/random" : $"jokes/random?category={category}";
            try
            {
                string reponse = await _httpClient.GetStringAsync(url);

                string joke = JsonConvert.DeserializeObject<dynamic>(reponse).value;

                if (firstname != null && lastname != null)
                {
                    return swapHero(joke, firstname, lastname);
                }

                return joke;

            } catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching jokes from {_httpClient.BaseAddress}: {ex.Message}.");
                return null;
            }
            catch (JsonReaderException)
            {
                Console.WriteLine($"Error converting response to joke.");
                return null;
            } catch (RuntimeBinderException) {
                Console.WriteLine($"Error converting response to joke.");
                return null;
            }
}

        private string swapHero(string joke, string firstname, string lastname)
        {
            return joke.Replace(_chuck, firstname, StringComparison.OrdinalIgnoreCase).Replace(_norris, lastname, StringComparison.OrdinalIgnoreCase);
        }


    }
}
