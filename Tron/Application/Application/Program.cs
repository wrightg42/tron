// Program.cs
// <copyright file="Program.cs"> This code is protected under the MIT License. </copyright>
using System;
using System.Net;
using Networking;
using Tron;

namespace Application
{
#if WINDOWS
    /// <summary>
    /// The main program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args"> Any arguments/commands that the program is run/compiled with. </param>
        public static void Main(string[] args)
        {
            Console.Title = "Tron";
            GameData.Tron = new TronGame(0, 1);
            Help();
            GetCommand();
        }

        /// <summary>
        /// Prints the help message into the console.
        /// </summary>
        public static void Help()
        {
            Console.WriteLine("Welcome to tron!\n");
            Console.WriteLine("How to play:");
            Console.WriteLine("The aim of the game is to drive your car without crashing.");
            Console.WriteLine("While driving you leave a trail of which will smash cars if driven into.");
            Console.WriteLine("Be the last car driving!\n");
            Console.WriteLine("Controls:");
            Console.WriteLine("W to go up");
            Console.WriteLine("A to go left");
            Console.WriteLine("S to go down");
            Console.WriteLine("D to go right");
            Console.WriteLine("Left shift to boost");
            Console.WriteLine("When playing local multiplayer player two will use:");
            Console.WriteLine("Arrow keys for directions");
            Console.WriteLine("Right control for boost.");
            Console.WriteLine("Player three will use:");
            Console.WriteLine("I, J, K, L to change direction");
            Console.WriteLine("B for boost.\n\n");
            Console.WriteLine("To play local multiplayer type 'local'");
            Console.WriteLine("To play online multiplayer type 'online'");
            Console.WriteLine("To see what local servers are presently available type 'lan list'");
            Console.WriteLine("To display this message again type 'help'");
            Console.WriteLine("To quit the game type 'quit'");
        }

        /// <summary>
        /// Gets and executes a command.
        /// </summary>
        public static void GetCommand()
        {
            while (true)
            {
                // Get a command
                Console.Write("Enter a command... ");
                string inp = Console.ReadLine();

                if (inp.ToLower() == "local")
                {
                    StartLocalGame();
                }
                else if (inp.ToLower() == "online")
                {
                    StartOnlineGame();
                }
                else if (inp.ToLower() == "lan list")
                {
                    SearchLanForServers();
                }
                else if (inp.ToLower() == "help")
                {
                    Help();
                }
                else if (inp.ToLower() == "quit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("That is not a valid command!");
                }
            }
        }

        /// <summary>
        /// Starts a game of local multiplayer tron.
        /// </summary>
        public static void StartLocalGame()
        {
            // Get amount of wins needed
            int pointsToWin;
            int? pointsToWinNullable = GetIntFromUser("What is the desired round win number to win the game?", 1, 30);
            if (pointsToWinNullable == null)
            {
                return;
            }
            else
            {
                pointsToWin = pointsToWinNullable.Value;
            }

            // Get amount of players wanted
            int players;
            int? playersNullable = GetIntFromUser("How many players do you want?", 2, 3);
            if (playersNullable == null)
            {
                return;
            }
            else
            {
                players = playersNullable.Value;
            }   

            // Setup the game data
            GameData.Client = null;
            GameData.LocalMultiPlayer = true;
            GameData.LocalPlayers = players;
            GameData.Tron = new TronGame(players, pointsToWin);
            GameData.Tron.ResetGame(true);

            // Start the game
            StartGame();
        }

        /// <summary>
        /// Gets an integer from the user.
        /// </summary>
        /// <returns> The integer. </returns>
        /// <remarks> If null is returned the user wishes to go back. </remarks>
        public static int? GetIntFromUser(string question, int min, int max)
        {
            Console.WriteLine("{0}\nEnter a whole integer above {1} and below {2} or b to go back:", question, min, max);
            while (true)
            {
                // Get the input
                string inp = Console.ReadLine();
                int parseRes;

                if (inp == "b")
                {
                    return null;
                }
                else if (int.TryParse(inp, out parseRes) && parseRes >= min && parseRes <= max)
                {
                    return int.Parse(inp);
                }
                else
                {
                    Console.WriteLine("Enter a whole integer above {0} and below {1} or b to go back:", min, max);
                }
            }
        }

        /// <summary>
        /// Starts an online game of tron.
        /// </summary>
        public static void StartOnlineGame()
        {
            while (true)
            {
                // Get ip from user
                IPAddress inputtedIp = GetIpFromUser();
                if (inputtedIp == null)
                {
                    // Exit method if user wises to go back
                    return;
                }
                else
                {
                    // Connect to server
                    Console.WriteLine("Connecting... ");
                    GameData.Client = new Client();
                    bool? connected = GameData.Client.Connect(inputtedIp);

                    // Write message
                    if (connected == null)
                    {
                        Console.WriteLine("Host not found!");
                    }
                    else if (connected == false)
                    {
                        Console.WriteLine("Server is full!");
                    }
                    else
                    {
                        Console.Write("Connected!\nWaiting for round to start! Press any key to disconnect... ");

                        // Wait until round has started, player has asked to disconnect or the player has been kicked by the server
                        while (true)
                        {
                            if (!GameData.Client.Connected)
                            {
                                // Kicked
                                Console.WriteLine("\nYou were kicked from the server.");
                                break;
                            }
                            else if (Console.KeyAvailable)
                            {
                                // User inputted key
                                Console.ReadKey(true);
                                Console.Write("\nDisconnecting... ");
                                GameData.Client.Disconnect(false);
                                Console.WriteLine("Disconnected!");
                                break;
                            }
                            else if (GameData.Client.Tron.Action != string.Empty)
                            {
                                // Start game
                                GameData.LocalMultiPlayer = false;
                                StartGame();
                                Console.WriteLine("\nDisconnected Safely!");
                                break;
                            }
                        }

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets an ip address from the user.
        /// </summary>
        /// <remarks> If null is returned the user wishes to go back. </remarks>
        public static IPAddress GetIpFromUser()
        {
            while (true)
            {
                Console.Write("Enter the IP address of the server of b to go back... ");

                // Get the input
                string inp = Console.ReadLine();
                IPAddress ip;

                if (inp == "b")
                {
                    return null;
                }
                else if (IPAddress.TryParse(inp, out ip))
                {
                    return ip;
                }
                else
                {
                    Console.WriteLine("Invalid IP address!");
                }
            }
        }

        /// <summary>
        /// Searches the local area network for servers and prints a list of them on the screen.
        /// </summary>
        public static void SearchLanForServers()
        {
            // Get list of servers
            Console.Write("Searching... ");
            IPAddress[] ips = GameData.Client.SearchForLanServers();
            Console.WriteLine("Found {0} servers at:\n", ips.Length);

            // Show servers
            foreach (IPAddress ip in ips)
            {
                Console.WriteLine(ip.ToString());
            }

            Console.WriteLine();
        } 

        /// <summary>
        /// Starts the game.
        /// </summary>
        public static void StartGame()
        {
            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
#endif
}
