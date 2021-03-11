using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JokeGenerator.names
{
    /// <summary>
    /// NamesPrivserv Name Generator implementation. see https://www.names.privserv.com/ to know more about the service
    /// </summary>
    public class NamesPrivservNameGen : INameGen
    {

        private readonly string _baseUrl = "https://www.names.privserv.com";
        private readonly HttpClient _httpClient;


        public NamesPrivservNameGen(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            this._httpClient.BaseAddress = new Uri(_baseUrl);
        }
        
        /// <summary>
        /// Uses names.priserv.com api to generate a random name.
        /// </summary>
        /// <returns>A random name</returns>
        public async Task<Tuple<string, string>> GetRandomNameAsync()
        {

            try
            {
                string result = await _httpClient.GetStringAsync("api");
                dynamic dynamicJson = JsonConvert.DeserializeObject<dynamic>(result);
                return  Tuple.Create(dynamicJson.name.ToString(), dynamicJson.surname.ToString());
            } catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching categories from {_httpClient.BaseAddress}: {ex.Message}.");
                return null;
            } catch (JsonReaderException)
            {
                Console.WriteLine($"Error converting response to firstName,lastName tuple.");
                return null;
            }

        }
    }
}
