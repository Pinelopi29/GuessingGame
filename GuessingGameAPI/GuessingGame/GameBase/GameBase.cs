using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuessingGame.GameBase;

namespace GuessingGame
{
    partial class Program
    {
        public abstract class GameBase
        {
            JsonIO JSON = new JsonIO();

            public virtual async Task<int> GetRandomNumber()
            {
                Random rnd = new Random();
                var randomNumber = rnd.Next(1, 100);
                return randomNumber;
            }

            public virtual async Task<List<Scoreboard>> UpdateScoreBoards(string username, int attempts, int seconds, DateTime date)
            {           
                //Getting the data from the scoreboard.txt file if exists.
                List<Scoreboard> sb_list = JSON.ReadFromJsonFile<List<Scoreboard>>("C:\\GuessingGame\\scoreboard.txt");

                // Creating new List of type Scoreboard if not already exists.
                if (sb_list == null)
                {
                    sb_list = new List<Scoreboard>();
                }

                //Creating a new instance of object Scoreboard
                Scoreboard scoreboard = new Scoreboard
                {
                    UserName = username,
                    Attempts = attempts,
                    Seconds = seconds,
                    DateTime = date
                };

                //ading the scoreboard object to the list
                sb_list.Add(scoreboard);

                //ordered the list by the attempts of the user and then by how many seconds they made in ascending order
                sb_list = sb_list.OrderBy(o => o.Attempts).ThenBy(o => o.Seconds).ToList();

                //Write the list of objects to the scoreboard.txt file.
                await JSON.WriteToJsonFileAsync<List<Scoreboard>>("C:\\GuessingGame\\scoreboard.txt", sb_list);
                return sb_list;
            }
        }
    }
}
