using System;
using System.Net.Http;
using JokeGenerator.names;
using JokeGenerator.jokes;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {

        static void Main(string[] args)
        {

            PrintBanner();

            INameGen nameGenerator = new NamesPrivservNameGen(new HttpClient());
            IJokeGen jokeGen = new ChuckNorrisJokeGen(new HttpClient());

            while (true)
            {

                string category = null;
                Tuple<string, string> randomName = null;

                Console.WriteLine("Press any key to get random jokes, x to exit the program");
                if (Console.ReadKey().KeyChar == 'x') break;
                Console.SetCursorPosition(0, Console.CursorTop);

                Console.WriteLine("Do you want to use a random name? y/n");
                if (Console.ReadKey().KeyChar == 'y')
                {
                    randomName = GetRandomName(nameGenerator);
                } else {
                    Console.WriteLine();
                }

                Console.WriteLine("Do you want to specify a category? y/n");
                if (Console.ReadKey().KeyChar == 'y')
                {
                    category = GetCategories(jokeGen);
                } else {
                    Console.WriteLine();
                }

                GetRandomJokes(jokeGen, category, randomName);

            }

            Console.WriteLine("\nThank you for laughing with GeoJokes, we'd love to hear your feedback at https://github.com/Geotab/geotab-development-assessment/issues");

        }

        private static Tuple<string, string> GetRandomName(INameGen nameGen)
        {

            Console.WriteLine("\nLoading random name...");
            var result = nameGen.GetRandomNameAsync().Result;
            if (result == null)
            {
                Console.WriteLine($"{nameof(nameGen)} did not return any values, the service is downgraded, but you might still be able to generate jokes with the default name.");
            }
            else
            {
                Console.WriteLine($"{result.Item1} {result.Item2} will now be used as the main character of the jokes");
            }

            return result;

        }

        private static string GetCategories(IJokeGen jokeGen)
        {
            Console.WriteLine("\nLoading jokes categories...");
            var categories = jokeGen.GetCategoriesAsync().Result;
            if (!categories.Any())
            {
                Console.WriteLine("Categories did not return any values, the service is downgraded, but you might still be able to generate jokes.");
                return null;
            }
            else
                Console.WriteLine($"{string.Join(", ", categories)}");


            Console.WriteLine("Enter a category name, then press Enter");
            var result = Console.ReadLine().ToLower();

            while (!categories.Any(x => x.Equals(result, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"{result} does not belong to the list of categories available. please enter a value in the following selection:");
                Console.WriteLine($"{string.Join(", ", categories)}");
                result = Console.ReadLine().ToLower();
            }

            return result;

        }


        private static void GetRandomJokes(IJokeGen jokeGen, string category, Tuple<string, string> names)
        {

            int count = 0;
            bool isValid = false;
            while (!isValid)
            {
                Console.WriteLine("How many jokes do you want? (1-9), then press Enter");
                string value = Console.ReadLine();
                if (int.TryParse(value, out count) && count > 0 && count < 10)
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine($"{value} is invalid, it should be a number between 1 and 9.");
                }
            }

            int maxRetry = 15;
            int attempt = 0;

            HashSet<string> displayedJokes = new HashSet<string>();

            for (int i = 0; i < count; i++)
            {
                var joke = jokeGen.GetRandomJokeAsync(names?.Item1, names?.Item2, category).Result;

                if(displayedJokes.Contains(joke))
                {
                    i--;
                    attempt++;
                }
                else
                {
                    Console.WriteLine(i + 1 + ": " + joke);
                    displayedJokes.Add(joke);
                }

                if(attempt >= maxRetry)
                {
                    Console.WriteLine($"GeoJokes AI was not able to find 9 jokes{(category == null ? "." : $" in {category} category")}, please contribute to our source code to build a better GeoJokes");
                    break;
                }
            }

        }

        private static void PrintBanner()
        {

            var arr = new[]
            {
                      @"    /$$$$$$                         /$$$$$           /$$                            ",
                      @"   /$$__  $$                       |__  $$          | $$                            ",
                      @"  | $$  \__/  /$$$$$$   /$$$$$$       | $$  /$$$$$$ | $$   /$$  /$$$$$$   /$$$$$$$  ",
                      @"  | $$ /$$$$ /$$__  $$ /$$__  $$      | $$ /$$__  $$| $$  /$$/ /$$__  $$ /$$_____/  ",
                      @"  | $$|_  $$| $$$$$$$$| $$  \ $$ /$$  | $$| $$  \ $$| $$$$$$/ | $$$$$$$$|  $$$$$$   ",
                      @"  | $$  \ $$| $$_____/| $$  | $$| $$  | $$| $$  | $$| $$_  $$ | $$_____/ \____  $$  ",
                      @"  |  $$$$$$/|  $$$$$$$|  $$$$$$/|  $$$$$$/|  $$$$$$/| $$ \  $$|  $$$$$$$ /$$$$$$$/  ",
                      @"   \______/  \_______/ \______/  \______/  \______/ |__/  \__/ \_______/|_______/   "
            };

            Console.WriteLine("\n");
            foreach (string line in arr)
                Console.WriteLine(line);
            Console.WriteLine("\n");

        }

    }
}
