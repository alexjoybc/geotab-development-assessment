using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JokeGenerator.names
{
    /// <summary>
    /// The IName generator interface defines functionality that name generator must implements to be compatible with geoJokes.
    /// </summary>
    public interface INameGen
    {
        /// <summary>
        /// Returns a tuple where item1 is the last name and item2 is the firstname
        /// </summary>
        /// <returns>A random name</returns>
        Task<Tuple<string, string>> GetRandomNameAsync();
    }
}
