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
                Tuple<string, string> randomNames = null;

                Console.WriteLine("Press any key to get random jokes, x to exit the program");
                if (Console.ReadKey().KeyChar == 'x') break;
                Console.SetCursorPosition(0, Console.CursorTop);

                Console.WriteLine("Do you want to use a random name? y/n");
                if (Console.ReadKey().KeyChar == 'y')
                {
                    randomNames = GetRandomName(nameGenerator);
                }

                Console.WriteLine("\nDo you want to specify a category? y/n");
                if (Console.ReadKey().KeyChar == 'y')
                {
                    category = GetCategories(jokeGen);
                }

                int n = 0;
                bool isValid = false;
                while (!isValid)
                {
                    Console.WriteLine("\nHow many jokes do you want? (1-9), then press Enter");
                    string value = Console.ReadLine();
                    if (int.TryParse(value, out n) && n > 0 && n < 10)
                    {
                        isValid = true;
                    }
                    else
                    {
                        Console.WriteLine($"{value} is invalid, it should be a number between 1 and 9.");
                    }
                }

                GetRandomJokes(jokeGen, category, n, randomNames);

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


        private static void GetRandomJokes(IJokeGen jokeGen, string category, int number, Tuple<String, String> names)
        {
            // To prevent DDOS attacks on joke service ;-)
            if (number > 9) number = 9;

            for (int i = 0; i < number; i++)
            {
                Console.WriteLine(jokeGen.GetRandomJokeAsync(names?.Item1, names?.Item2, category).Result);
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
