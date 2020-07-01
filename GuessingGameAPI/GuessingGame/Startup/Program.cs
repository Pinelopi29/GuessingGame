using System;
using Serilog;
using System.Threading.Tasks;

namespace GuessingGame
{
    partial class Program
    {
        static async Task Main(string[] args)
        {
            //An ILogger is created using LoggerConfiguration.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("C:\\GuessingGame\\app_logs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            try
            {
                Log.Information("Starting up");
                //Creating an instance of IGame interface which is inherited by GuessingGameConsole (includes the logic of tha game)
               
               //IGame game1 = new GuessingGameConsole(30000);
               //passing 30 seconds to run method
               // game1.RunAsync();

                IGame game1 = new GuessingGameHttp(30000);
                // passing 30 seconds to run method
                await game1.RunAsync();
            } 
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
