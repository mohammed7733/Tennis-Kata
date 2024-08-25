namespace Tennis;

public class TennisGame4  : ITennisGame
{
    internal int player1Score;
    internal int player2Score;
    internal readonly string player1Name;
    internal readonly string player2Name;
    private static readonly string[] Scores = {"Love", "Fifteen", "Thirty", "Forty"};

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
        TennisResult result = GetResult();
        return FormatResult(result);
    }

    private TennisResult GetResult() {
        
        if (IsDeuce())
            return new TennisResult("Deuce", "");
        if (ServerHasWon())
            return new TennisResult("Win for " + player1Name, "");
        if(ReceiverHasWon())
            return new TennisResult("Win for " + player2Name, "");
        if (ServerHasAdvantage())
            return new TennisResult("Advantage " + player1Name, "");
        if (ReceiverHasAdvantage())
            return new TennisResult("Advantage " + player2Name, "");
        return new TennisResult(Scores[player1Score], Scores[player2Score]);
    }

    private string FormatResult(TennisResult result)
    {
        if ("".Equals(result.Player2Score))
            return result.Player1Score;
        if (IsDraw())
            return DrawScore(result);
        return result.Player1Score + "-" + result.Player2Score;
    }

    private static string DrawScore(TennisResult result)
    {
        return result.Player1Score + "-All";
    }

    private bool IsDraw()
    {
        return player1Score.Equals(player2Score);
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

internal record TennisResult(string Player1Score, string Player2Score);

internal class Game {
    private readonly TennisGame4 _game;

    public Game(TennisGame4 game) {
        _game = game;
    }
}