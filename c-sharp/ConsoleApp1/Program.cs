using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static string[] results = new string[50];
        static char key;
        static Tuple<string, string> names;

        static void Main(string[] args)
        {

            PrintBanner();

            string category = null;

            while (true)
            {

                Console.WriteLine("Press any key to get random jokes");
                Console.ReadKey();
                Console.SetCursorPosition(0, Console.CursorTop);

                Console.WriteLine("Want to use a random name? y/n");
                GetEnteredKey(Console.ReadKey());
                Console.WriteLine();

                if (key == 'y')
                    GetNames();

                Console.WriteLine("Want to specify a category? y/n");
                GetEnteredKey(Console.ReadKey());
                Console.WriteLine();

                if (key == 'y')
                {
                    Console.WriteLine("Loading available categories... (be patient)");
                    GetCategories();

                    Console.WriteLine("Enter a category, then press Enter");
                    category = Console.ReadLine();

                }

                Console.WriteLine("How many jokes do you want? (1-9), then press Enter");
                int n = Int32.Parse(Console.ReadLine());
                GetRandomJokes(category, n);

                category = null;
                names = null;
            }

        }

        private static void GetEnteredKey(ConsoleKeyInfo consoleKeyInfo)
        {
            key = consoleKeyInfo.KeyChar;
        }

        private static void GetRandomJokes(string category, int number)
        {

            JsonFeed jsonFeed = new JsonFeed("https://api.chucknorris.io");

            for (int i = 0; i < number; i++)
            {
                Console.WriteLine(jsonFeed.GetRandomJokes(names?.Item1, names?.Item2, category));
            }

        }

        private static void GetCategories()
        {
            JsonFeed jsonFeed = new JsonFeed("https://api.chucknorris.io/jokes/categories");
            Console.WriteLine(string.Join(",", jsonFeed.GetCategories()));
        }

        private static void GetNames()
        {
            JsonFeed jsonFeed = new JsonFeed("https://www.names.privserv.com/api/");
            dynamic result = jsonFeed.Getnames();
            names = Tuple.Create(result.name.ToString(), result.surname.ToString());
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
