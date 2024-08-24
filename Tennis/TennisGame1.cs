using System;

namespace Tennis
{
    public class TennisGame1 : ITennisGame
    {
        private int player1Score = 0;
        private int player2Score = 0;
        private string player1Name;
        private string player2Name;
        private string winner;
        private string[] scores = { "Love", "Fifteen", "Thirty", "Forty" };

        public TennisGame1(string player1Name, string player2Name)
        {
            this.player1Name = player1Name;
            this.player2Name = player2Name;
            this.winner = player1Name;
        }

        public void WonPoint(string playerName)
        {
            if (playerName == "player1")
                player1Score += 1;
            else
                player2Score += 1;
            winner = player1Score >= player2Score ? player1Name : player2Name;
        }

        public string GetScore()
        {
            if (IsDrawScore())
            {
                return DrawScore();
            }

            if (IsAbove4Score())
            {
                return Above4Score();
            }

            return Below4Score();
        }

        private bool IsAbove4Score()
        {
            return player1Score >= 4 || player2Score >= 4;
        }

        private bool IsDrawScore()
        {
            return player1Score == player2Score;
        }

        private string Below4Score() => scores[player1Score] + "-" + scores[player2Score];

        private string Above4Score() =>
            Math.Abs(player1Score - player2Score) == 1 ? "Advantage " + winner : "Win for " + winner;

        private string DrawScore()
        {
            if (player1Score >= 3)
                return "Deuce";
            return scores[player1Score] + "-All";
        }
    }
}

