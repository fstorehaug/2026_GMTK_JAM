using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;


public static class TournamentState
{
    public static int LeftPlayerID = 0;
    public static int RightPlayerID = 1;
    public static List<TournamentPlayer> players { get; private set; }
    // Static reference readable by any script, but writable only by this class

    static TournamentState()
    {
        players= new List<TournamentPlayer>() {new TournamentPlayer("Snailio"), new TournamentPlayer("Snailis")};

    }

 
    public static void updatePlayerScore (int playerNumber, float addedScore)
    {
        players[playerNumber].totalScore += addedScore;
    }

    public static void incrementPlayerShot(int playerNumber)
    {
        players[playerNumber].timesShot += 1;
    }

    public static void updatePlayerAppearance(int playerNumber, string appearance)
    {
        players[playerNumber].appearance =appearance;
    }
}