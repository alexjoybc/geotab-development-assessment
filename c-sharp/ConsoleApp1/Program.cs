﻿using System;
using System.Net.Http;
using JokeGenerator.names;
using JokeGenerator.jokes;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {

        static char key;

        static void Main(string[] args)
        {

            PrintBanner();

            string category = null;

            INameGen nameGenerator = new NamesPrivservNameGen(new HttpClient());
            IJokeGen jokeGen = new ChuckNorrisJokeGen(new HttpClient());

            while (true)
            {

                Tuple<string, string> randomNames = null;

                Console.WriteLine("Press any key to get random jokes");
                Console.ReadKey();
                Console.SetCursorPosition(0, Console.CursorTop);

                Console.WriteLine("Want to use a random name? y/n");
                if (Console.ReadKey().KeyChar == 'y')
                {
                    Console.WriteLine("\nLoading random name...");
                    randomNames = nameGenerator.GetRandomNameAsync().Result;
                }

                Console.WriteLine("Want to specify a category? y/n");
                if (Console.ReadKey().KeyChar == 'y' && PrintCategories(jokeGen))
                {
                    Console.WriteLine("Enter a category, then press Enter");
                    category = Console.ReadLine();
                }

                Console.WriteLine("How many jokes do you want? (1-9), then press Enter");
                int n = Int32.Parse(Console.ReadLine());
                GetRandomJokes(category, n, randomNames);

                category = null;

            }

        }

        private static Tuple<string, string> getRandomName(INameGen nameGen)
        {
            var result = nameGen.GetRandomNameAsync().Result;

            if(result == null)
            {
                Console.WriteLine($"{nameof(nameGen)} did not return any values, the service is downgraded, but you might still be able to generate jokes with the default name.");
            }

            return result;

        }

        private static void GetRandomJokes(string category, int number, Tuple<String, String> names)
        {

            JsonFeed jsonFeed = new JsonFeed("https://api.chucknorris.io");

            for (int i = 0; i < number; i++)
            {
                Console.WriteLine(jsonFeed.GetRandomJokes(names?.Item1, names?.Item2, category));
            }

        }

        private static bool PrintCategories(IJokeGen jokeGen)
        {
            var result = jokeGen.GetCategoriesAsync().Result;
            if (!result.Any())
            {
                Console.WriteLine("\nCategories did not return any values, the service is downgraded, but you might still be able to generate jokes.");
                return false;
            }
            else
                Console.WriteLine($"\n{string.Join(", ", result)}");

            return true;
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
