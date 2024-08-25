using System;

namespace Tennis
{
    public class TennisGame1 : ITennisGame
    {
        private Player _player1;
        private Player _player2;

        public TennisGame1(string player1Name, string player2Name)
        {
            _player1 = new Player(player1Name);
            _player2 = new Player(player2Name);
        }

        public void WonPoint(string playerName)
        {
            if (playerName == "player1")
            {
                _player1.WonPoint();
            }
            else
            {
                _player2.WonPoint();
            }
        }

        public string GetScore()
        {
            if (IsDrawScore())
            {
                return _player1.Score().DrawScore();
            }

            if (_player1.Score().IsAbove3())
            {
                return Above3Score(_player1);
            }

            if (_player2.Score().IsAbove3())
            {
                return Above3Score(_player2);
            }

            return Below4Score();
        }

        private string Above3Score(Player player)
        {
            return IsAdvantageScore()? player.GetAdvantageScore() : WinnerScore();
        }

        private bool IsDrawScore()
        {
            return _player1.Score().Value() == _player2.Score().Value();
        }

        private string Below4Score() => _player1.Score().Name() + "-" + _player2.Score().Name();

        private string WinnerScore() => "Win for " + (_player1.HasHigherScore(_player2) ? _player1 : _player2).GetName();

        private bool IsAdvantageScore() => _player1.Score().IsAdvantageScore(_player2.Score());
    }

    public class Player
    {
        private Score _score;
        private string _name;

        public Player(string name)
        {
            _score = new Score();
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

        public void WonPoint()
        {
            _score.Increment();
        }

        public Score Score()
        {
            return _score;
        }

        public bool HasHigherScore(Player player2)
        {
            return Score().Value() > player2.Score().Value();
        }

        public string GetAdvantageScore() => "Advantage " + GetName();
    }

    public class Score
    {
        private int _value;

        public void Increment()
        {
            _value++;
        }

        public string Name()
        {
            return _value switch
            {
                0 => "Love",
                1 => "Fifteen",
                2 => "Thirty",
                _ => "Forty"
            };
        }

        public int Value()
        {
            return _value;
        }

        public bool IsAbove3()
        {
            return _value >= 4;
        }

        public bool IsAdvantageScore(Score player2Score)
        {
            return Math.Abs(_value - player2Score.Value()) == 1;
        }

        public bool IsDeuceScore()
        {
            return _value >= 3;
        }

        public string DrawScore()
        {
            if (IsDeuceScore())
                return "Deuce";
            return Name() + "-All";
        }
    }
}

