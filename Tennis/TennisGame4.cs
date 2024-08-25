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
        TennisResult result = new Deuce(
            this, new GameServer(
                this, new GameReceiver(
                    this, new AdvantageServer(
                        this, new AdvantageReceiver(
                            this, new DefaultResult(this)))))).GetResult();
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

internal class Deuce : IResultProvider {
    private readonly TennisGame4 _game;
    private readonly IResultProvider _nextResult;

    public Deuce(TennisGame4 game, IResultProvider nextResult) {
        _game = game;
        _nextResult = nextResult;
    }

    public TennisResult GetResult() {
        if (_game.IsDeuce())
            return new TennisResult("Deuce", "");
        return _nextResult.GetResult();
    }
}

internal class GameServer : IResultProvider {
    private readonly TennisGame4 _game;
    private readonly IResultProvider _nextResult;

    public GameServer(TennisGame4 game, IResultProvider nextResult) {
        _game = game;
        _nextResult = nextResult;
    }

    public TennisResult GetResult() {
        if (_game.ServerHasWon())
            return new TennisResult("Win for " + _game.player1Name, "");
        return _nextResult.GetResult();
    }
}

internal class GameReceiver : IResultProvider {
    private readonly TennisGame4 _game;
    private readonly IResultProvider _nextResult;

    public GameReceiver(TennisGame4 game, IResultProvider nextResult) {
        _game = game;
        _nextResult = nextResult;
    }

    public TennisResult GetResult() {
        if (_game.ReceiverHasWon())
            return new TennisResult("Win for " + _game.player2Name, "");
        return _nextResult.GetResult();
    }
}

internal class AdvantageServer : IResultProvider {
    private readonly TennisGame4 _game;
    private readonly IResultProvider _nextResult;

    public AdvantageServer(TennisGame4 game, IResultProvider nextResult) {
        _game = game;
        _nextResult = nextResult;
    }

    public TennisResult GetResult() {
        if (_game.ServerHasAdvantage())
            return new TennisResult("Advantage " + _game.player1Name, "");
        return _nextResult.GetResult();
    }
}

internal class AdvantageReceiver : IResultProvider {

    private readonly TennisGame4 _game;
    private readonly IResultProvider _nextResult;

    public AdvantageReceiver(TennisGame4 game, IResultProvider nextResult) {
        _game = game;
        _nextResult = nextResult;
    }

    public TennisResult GetResult() {
        if (_game.ReceiverHasAdvantage())
            return new TennisResult("Advantage " + _game.player2Name, "");
        return _nextResult.GetResult();
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
