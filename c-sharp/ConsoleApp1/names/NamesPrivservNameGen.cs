using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JokeGenerator
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


        public async Task<Tuple<string, string>> GetRandomNameAsync()
        {

            try
            {
                string result = await _httpClient.GetStringAsync("api");
                dynamic dynamicJson = JsonConvert.DeserializeObject<dynamic>(result);
                return  Tuple.Create(dynamicJson.name.ToString(), dynamicJson.surname.ToString());
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            

        }
    }
}
