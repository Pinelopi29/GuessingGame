using System;
using System.Timers;

namespace GuessingGame
{

        public class Timer 
        {
            //define an eventHandler object
            public event EventHandler OnNumberChanged;

            //setting a timer
            private System.Timers.Timer NumberGeneratorTimer;

            //constractor starts the timer
            public Timer(int replacetime)
            {
                SetTimer(replacetime);
            }

            //Creating a timer with 30 seconds interval
            private void SetTimer(int replacetime)
            {
                // Create a timer with 30 second interval.
                NumberGeneratorTimer = new System.Timers.Timer(replacetime);
                // Hook up the Elapsed event for the timer. 
                NumberGeneratorTimer.Elapsed += OnTick;
                NumberGeneratorTimer.AutoReset = true;
                NumberGeneratorTimer.Enabled = true;
            }

            //Reset the timer
            public void ResetTimer()
            {
                NumberGeneratorTimer.AutoReset = true;
                NumberGeneratorTimer.Enabled = true;
            }

            //Stop the timer
            public void StopTimer()
            {
                NumberGeneratorTimer.Enabled = false;
            }

            //Triggers an event which will regenerated a random number each time the event elapsed.
            private void OnTick(object sender, ElapsedEventArgs E)
            {
                OnNumberChanged?.Invoke(this, EventArgs.Empty);
            }

        }

    

}

