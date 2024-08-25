using System;

namespace Tennis;

public class TennisGame4  : ITennisGame
{
    private int player1Score;
    private int player2Score;
    private readonly string player1Name;
    private readonly string player2Name;
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
        if (IsAdvantageScore())
            return new TennisResult("Advantage " + playerWithHigherScore(), "");
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

    private string playerWithHigherScore()
    {
        return player1Score >= player2Score ? player1Name : player2Name;
    }

    private static string DrawScore(TennisResult result)
    {
        return result.Player1Score + "-All";
    }

    private bool IsDraw()
    {
        return player1Score.Equals(player2Score);
    }

    private bool IsAdvantageScore() {
        return IsAboveForty() && Math.Abs(player1Score - player2Score) == 1;
    }

    private bool IsAboveForty()
    {
        return player1Score >= 4 || player2Score >= 4;
    }

    private bool ReceiverHasWon() {
        return player2Score >= 4 && (player2Score - player1Score) >= 2;
    }

    private bool ServerHasWon() {
        return player1Score >= 4 && (player1Score - player2Score) >= 2;
    }

    private bool IsDeuce() {
        return player1Score >= 3 && player2Score >= 3 && (player1Score == player2Score);
    }
}

internal record TennisResult(string Player1Score, string Player2Score);