using System;
using System.Collections.Generic;
using System.Text;

namespace GuessingGame
{
    class Scoreboard
    {
        public string UserName { get; set; }
        public int Attempts { get; set; }
        public int Seconds { get; set; }
        public DateTime DateTime { get; set; }
        public override string ToString()
        {
            return UserName + " " + Attempts + "tries " + Seconds + " sec" + " on " + DateTime;
        }

    }

}
