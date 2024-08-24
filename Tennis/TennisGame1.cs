using System;

namespace Tennis
{
    public class TennisGame1 : ITennisGame
    {
        private Player _player1;
        private Player _player2;
        private string player1Name;
        private string player2Name;
        private string winner;

        public TennisGame1(string player1Name, string player2Name)
        {
            this.player1Name = player1Name;
            this.player2Name = player2Name;
            this.winner = player1Name;
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

            winner = _player1.Score().Value() >= _player2.Score().Value() ? player1Name : player2Name;
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
            return _player1.Score().IsAboveOrEqual4() || _player2.Score().IsAboveOrEqual4();
        }

        private bool IsDrawScore()
        {
            return _player1.Score().Value() == _player2.Score().Value();
        }

        private string Below4Score() => _player1.Score().Name() + "-" + _player2.Score().Name();

        private string Above4Score() =>
            IsAdvantageScore() ? AdvantageScore() : WinnerScore();

        private string WinnerScore() => "Win for " + winner;

        private string AdvantageScore() => "Advantage " + winner;

        private bool IsAdvantageScore() => Math.Abs(_player1.Score().Value() - _player2.Score().Value()) == 1;

        private string DrawScore()
        {
            if (_player1.Score().Value() >= 3)
                return "Deuce";
            return _player1.Score().Name() + "-All";
        }
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
    }

    public class Score
    {
        private int _value;

        public Score()
        {
            _value = 0;
        }

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

        public bool IsAboveOrEqual4()
        {
            return Value() >= 4;
        }
    }
}

