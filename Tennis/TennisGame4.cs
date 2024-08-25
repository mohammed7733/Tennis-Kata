namespace Tennis;

public class TennisGame4  : ITennisGame
{
    internal int player1Score;
    internal int player2Score;
    internal readonly string player1Name;
    internal readonly string player2Name;

    public TennisGame4(string player1Name, string player2Name)
    {
        this.player1Name = player1Name;
        this.player2Name = player2Name;
    }

    public void WonPoint(string playerName)
    {
        if (player1Name.Equals(playerName))
            player1Score += 1;
        else
            player2Score += 1;
    }

    public string GetScore()
    {
        TennisResult result = new Game(this).GetResult();
        return result.Format();
    }

    internal bool ReceiverHasAdvantage() {
        return player2Score >= 4 && (player2Score - player1Score) == 1;
    }

    internal bool ServerHasAdvantage() {
        return player1Score >= 4 && (player1Score - player2Score) == 1;
    }

    internal bool ReceiverHasWon() {
        return player2Score >= 4 && (player2Score - player1Score) >= 2;
    }

    internal bool ServerHasWon() {
        return player1Score >= 4 && (player1Score - player2Score) >= 2;
    }

    internal bool IsDeuce() {
        return player1Score >= 3 && player2Score >= 3 && (player1Score == player2Score);
    }
}

internal class TennisResult {
    readonly string _serverScore;
    readonly string _receiverScore;

    public TennisResult(string serverScore, string receiverScore) {
        _serverScore = serverScore;
        _receiverScore = receiverScore;
    }

    internal string Format() {
        if ("".Equals(_receiverScore))
            return _serverScore;
        if (IsDrawScore())
            return DrawScore();
        return _serverScore + "-" + _receiverScore;
    }

    private string DrawScore()
    {
        return _serverScore + "-All";
    }

    private bool IsDrawScore()
    {
        return _serverScore.Equals(_receiverScore);
    }
}

internal interface IResultProvider {
    TennisResult GetResult();
}

internal class Game : IResultProvider {
    private readonly TennisGame4 _game;
    private static readonly string[] Scores = {"Love", "Fifteen", "Thirty", "Forty"};

    public Game(TennisGame4 game) {
        _game = game;
    }

    public TennisResult GetResult() {
        
        if (_game.IsDeuce())
            return new TennisResult("Deuce", "");
        if (_game.ServerHasWon())
            return new TennisResult("Win for " + _game.player1Name, "");
        if(_game.ReceiverHasWon())
            return new TennisResult("Win for " + _game.player2Name, "");
        if (_game.ServerHasAdvantage())
            return new TennisResult("Advantage " + _game.player1Name, "");
        if (_game.ReceiverHasAdvantage())
            return new TennisResult("Advantage " + _game.player2Name, "");
        return new TennisResult(Scores[_game.player1Score], Scores[_game.player2Score]);
    }
}

internal class DefaultResult : IResultProvider {

    private static readonly string[] Scores = {"Love", "Fifteen", "Thirty", "Forty"};

    private readonly TennisGame4 _game;

    public DefaultResult(TennisGame4 game) {
        _game = game;
    }

    public TennisResult GetResult() {
        return new TennisResult(Scores[_game.player1Score], Scores[_game.player2Score]);
    }
}
