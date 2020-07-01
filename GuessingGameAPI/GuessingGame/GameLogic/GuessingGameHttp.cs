using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Serilog;
using System.Threading.Tasks;

namespace GuessingGame
{
    partial class Program
    {
        public class GuessingGameHttp : GameBase, IGame
        {
            private int RandomNumber;
            private List<Scoreboard> sb_list;
            private Timer timer;
            public HttpClient webClient;

            public GuessingGameHttp(int timeinterval)
            {
                timer = new Timer(timeinterval);
                webClient = new HttpClient();
            }

            public async Task RunAsync()
            {
                Log.Information("Now writing from GuessingGameHttp class..");

                RandomNumber = await GetRandomNumber();

                Log.Information("First random number is now generated -> {A}", RandomNumber);

                //assigned on OnNumberChanged the address of the fucntion Rn_GenerateNewRandom
                timer.OnNumberChanged += Rn_GenerateNewRandom;
                var watch = System.Diagnostics.Stopwatch.StartNew(); //creating a new stopwatch to count the seconds of the user been playing
                char playagain = 'y';

                do
                {
                    int playerid = 0;
                    var attempts = 0; // attempts of the user is equal with 0 before the game starts.
                    var foundNumber = false;
                    watch.Start(); //Starting the stopwatch each time we repeat the loop
                    Console.WriteLine("Please guess a number between 1 and 100");
                    Console.WriteLine("Random number generated, start! ");

                    //while the user has less than 10 attempts && didn't find the random number, continue playing.
                    while (attempts < 10 && foundNumber == false)
                    {
                        try
                        {
                            var inputNumber = Convert.ToInt32(Console.ReadLine()); //geting the random number from the user
                            Log.Debug("User inserting a number -> {A}", inputNumber);

                            if (inputNumber == Convert.ToInt32(RandomNumber))
                            {
                                //if the input number of the user == with the random generated number..
                                foundNumber = true;

                                watch.Stop(); //stop the timer
                                timer.StopTimer(); //stop the timer running in the background
                                Console.WriteLine("Bingo!!! The Magic number was " + RandomNumber);
                                Console.WriteLine("Insert your details for the score board.");
                                Console.WriteLine("Name: ");
                                var username = Console.ReadLine(); //getting the username of the user

                                //passing username, attempts, seconds, and the dataTime to the updateScoreBoards method 
                                //to store in the file the user's attempt.
                                sb_list = await UpdateScoreBoards(username, attempts, (int)(watch.ElapsedMilliseconds / 1000), DateTime.Now);
                                foreach (Scoreboard item in sb_list)
                                {
                                    playerid++;
                                    Console.WriteLine(playerid + ". " + item.ToString());
                                }
                                watch.Reset(); //reset the stopwatch
                            }

                            //if the user made less than 10 attempts, give hints,
                            //else display a failed message to the user
                            else if (attempts < 9)
                            {
                                if (inputNumber > Convert.ToInt32(RandomNumber))
                                {
                                    Console.WriteLine("Smaller");
                                }
                                else
                                {
                                    Console.WriteLine("Greater");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Oops, maybe next time.");
                                Log.Information("User faild to find the random number, made 10 attempts");
                            }
                        }
                        catch (Exception ex)

                        {
                            Console.WriteLine("Please enter a number");
                            Log.Error(ex, "Something went wrong, please enter a valid number");
                        }
                        attempts++;
                    }

                    timer.StopTimer(); //stop the timer running in the background
                    Console.WriteLine("Do you want to play again? y/n");  //ask the user if wants to play again              
                    watch.Reset(); //reset the stopwatch

                    playagain = Console.ReadKey().KeyChar; //get the input from the user y/n
                    Log.Information("User now inserting y or n-> {A}", playagain);
                    Console.WriteLine();

                    //check for invalid input
                    while (playagain != 'y' && playagain != 'Y' && playagain != 'n' && playagain != 'N')
                    {
                        Console.WriteLine();
                        Log.Error("User inserted an invalid input-> {A}", playagain);
                        Console.WriteLine("Please type y or n");
                        playagain = Console.ReadKey().KeyChar;
                    }
                    timer.ResetTimer(); //reset the timer running in the background
                    RandomNumber = await GetRandomNumber(); //get a new random number

                } while (playagain == 'y' || playagain == 'Y');

                Log.CloseAndFlush();
                Console.WriteLine("Bye :D");
            }

            //returns a new random number each 30 secs passed in game
            async void Rn_GenerateNewRandom(object sender, EventArgs e)
            {
                Log.Information("30 secs passed, user faild to find the generated number");
                RandomNumber = await GetRandomNumber();
                Console.WriteLine("Oops, 30 seconds passed, new random number generated->{0}", RandomNumber);
                Console.WriteLine("Start again: ");
            }

            public async override Task<List<Scoreboard>> UpdateScoreBoards(string username, int attempts, int seconds, DateTime date)
            {
                HttpResponseMessage response = null;

                //Creating a new instance of object Scoreboard
                Scoreboard scoreboard = new Scoreboard
                {
                    UserName = username,
                    Attempts = attempts,
                    Seconds = seconds,
                    DateTime = date
                };
                var json = JsonConvert.SerializeObject(scoreboard);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = "http://localhost:54425/api/Scoreboard";

                try
                {
                    response = await webClient.PostAsync(url, data);                  
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong: " + e.Message.ToString());
                }                       
                     var readTask = response.Content.ReadAsAsync<List<Scoreboard>>();
                     sb_list = readTask.Result;                
                return sb_list;
            }

            public async override   Task<int> GetRandomNumber()
            {
                Console.WriteLine("in http get random");
                HttpResponseMessage response = null;            
                var url = "http://localhost:54425/api/RandomNumber";

                try
                {
                    response = await webClient.GetAsync(url);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong: " + e.Message.ToString());
                    if (e.InnerException != null)
                    {
                        Console.WriteLine(e.InnerException.Message);
                    }
                }              
                RandomNumber = await response.Content.ReadAsAsync<int>();                   
                return RandomNumber;
            }       
        }
    }
}