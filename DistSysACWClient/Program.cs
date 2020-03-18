using System;
using System.Diagnostics;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DistSysACWClient
{
    
    class Client
    {
        public Client()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Debug.WriteLine(args.ExceptionObject.GetType());
                Console.WriteLine("An unexpected error occured please try again.");
            };
        }
        
        static async Task Main(string[] args)
        {
            const string address = "http://localhost:5000/api";
            var talkBackHandler = new TalkBackHandler(address);
            var userHandler = new UserHandler(address);
            var protectedHandler = new ProtectedHandler(address);
            Console.WriteLine("Hello. What would you like to do?");
            var input = Console.ReadLine();
            bool handled = false;

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
                    handled = true;
                }

                if (input.Contains("User Get"))
                {
                    await userHandler.UserGet(input);
                    handled = true;
                }

                if (input.Contains("User Post"))
                {
                    await userHandler.UserPost(input);
                    handled = true;
                }

                if (input.Contains("User Set"))
                {
                    await userHandler.UserSet(input);
                    handled = true;
                }

                if (input.Contains("User Role"))
                {
                    await userHandler.ChangeUserRole(input);
                    handled = true;
                }

                if (input.Contains("Protected SHA1"))
                {
                    await protectedHandler.Sha1(input);
                    handled = true;
                }
                
                if (input.Contains("Protected SHA256"))
                {
                    await protectedHandler.Sha256(input);
                    handled = true;
                }

                if (!handled)
                {
                    switch (input)
                    {
                        case "TalkBack Hello":
                            await talkBackHandler.Hello();
                            break;
                        case "User Delete":
                            await userHandler.DeleteUser();
                            break;
                        case "Protected Hello":
                            await protectedHandler.Hello();
                            break;
                        default:
                            Console.WriteLine("Input not recognised");
                            break;
                    }    
                }


                handled = false;
                Console.WriteLine("What would you like to do next?");
                input = Console.ReadLine();
                Console.Clear();
            }
        }
    }
}