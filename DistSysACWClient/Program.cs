using System;
using System.Threading.Tasks;

namespace DistSysACWClient
{
    #region Task 10 and beyond
    class Client
    {
        static async Task Main(string[] args)
        {
            const string address = "http://localhost:5000/api";
            var talkBackHandler = new TalkBackHandler(address);
            Console.WriteLine("Hello. What would you like to do?");
            var input = Console.ReadLine();

            while (true)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Please enter something to do");
                    continue;
                }

                if (input.Equals("Exit"))
                    break;

                if (input.Contains("TalkBack Sort"))
                {
                    await talkBackHandler.Sort(input);
                    Console.WriteLine("What would you like to do next?");
                    input = Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                switch (input)
                {
                    case "TalkBack Hello":
                        await talkBackHandler.Hello();
                        break;
                    default:
                        Console.WriteLine("Input not recognized.");
                        break;
                }
                
                Console.WriteLine("What would you like to do next?");
                input = Console.ReadLine();
                Console.Clear();
            }
        }
    }
    #endregion
}