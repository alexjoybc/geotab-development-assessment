using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JokeGenerator.jokes
{
    /// <summary>
    /// The IJokeGen interface defines functionality that joke generators must implement
    /// </summary>
    public interface IJokeGen
    {
        /// <summary>
        /// Returns a list of available categories for the generator.
        /// When implementing this service it is recomended to return an empty list on known error and print a message.
        /// </summary>
        /// <returns>A List of string</returns>
        Task<IEnumerable<String>> GetCategoriesAsync();

        /// <summary>
        /// Returns a random joke, if firstname and lastname is provided, the main character must be switched, an optional filter category can be provided
        /// </summary>
        /// <param name="firstname">A character firstName</param>
        /// <param name="lastname">A character lastName</param>
        /// <param name="category">A category</param>
        /// <returns></returns>
        Task<string> GetRandomJokeAsync(string firstname, string lastname, string category);

    }
}
