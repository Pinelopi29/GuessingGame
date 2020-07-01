using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuessingGame;

namespace GuessingGameApp.Interfaces
{
    public interface IGuessingGameService
    {
        public int GetRandomNumber();
        public List<Scoreboard> UpdateScoreBoards(string username, int attempts, int seconds, DateTime date);

    }
}
