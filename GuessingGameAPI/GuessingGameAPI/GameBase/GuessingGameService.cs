using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GuessingGame;
using GuessingGameApp.Interfaces;
using Newtonsoft.Json;

namespace GuessingGameApp.GameBase
{
    public class GuessingGameService : IGuessingGameService
    {
        public int GetRandomNumber()
        {
            Random rnd = new Random();
            var randomNumber = rnd.Next(1, 100);
            return randomNumber;
        }

        public List<Scoreboard> UpdateScoreBoards(string username, int attempts, int seconds, DateTime date)
        {          
            //Getting the data from the scoreboard.txt file if exists.
            List<Scoreboard> sb_list = ReadFromJsonFile<List<Scoreboard>>("C:\\GuessingGame\\scoreboardHTTP.txt");

            // Creating new List of type Scoreboard if not already exists.
            if (sb_list == null)
            {
                sb_list = new List<Scoreboard>();
            }

            //Creating a new instance of object Scoreboard
            Scoreboard scoreboard = new Scoreboard
            {
                UserName = username,
                Attempts = attempts + 1,
                Seconds = seconds,
                DateTime = date
            };


            //ading the scoreboard object to the list
            sb_list.Add(scoreboard);

            //ordered the list by the attempts of the user and then by how many seconds they made in ascending order
            sb_list = sb_list.OrderBy(o => o.Attempts).ThenBy(o => o.Seconds).ToList();

            //Write the list of objects to the scoreboard.txt file.
            WriteToJsonFile<List<Scoreboard>>("C:\\GuessingGame\\scoreboardHTTP.txt", sb_list);
           
            return sb_list;
        }

        public void WriteToJsonFile<Scoreboard>(string filePath, Scoreboard objectToWrite, bool append = false) where Scoreboard : new()
        {

            //creating a TextWriter.
            TextWriter writer = null;
            try
            {

                //serialize the list of scoreboard object in order to save them on the txt file
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Could not write to text file because of error: " + ex.Message.ToString());

            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }


        public Scoreboard ReadFromJsonFile<Scoreboard>(string filePath) where Scoreboard : new()
        {
            //Creating a TextReader.
            TextReader reader = null;
            try
            {
                //read the file of the specific filepath till the end 
                //Deserialize JSON data in order to be able to read them.
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Scoreboard>(fileContents);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Could not read text file because of error: " + ex.Message.ToString());
                return default(Scoreboard);
            }

            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
