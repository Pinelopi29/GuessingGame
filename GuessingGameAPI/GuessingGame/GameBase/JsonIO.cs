using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GuessingGame.GameBase
{
    class JsonIO
    {
        public async Task WriteToJsonFileAsync<Scoreboard>(string filePath, Scoreboard objectToWrite, bool append = false) where Scoreboard : new()
        {

            //creating a TextWriter.
            TextWriter writer = null;
            try
            {
                //serialize the list of scoreboard object in order to save them on the txt file
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(filePath, append);
                await writer.WriteAsync(contentsToWriteToFile);
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
