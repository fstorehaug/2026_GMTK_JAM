using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class TournamentPlayer
{
    public string name = "Anonymous Snail";
    public int timesShot = 0;
    public float bestShot = 0;
    public float totalScore = 0;
    public string appearance = "aaa";
    public TournamentPlayer(string newName)
    {
        name = newName;
        /** appearance code
        string tmpStr = "";
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        tmpStr += chars[Random.Shared.Next(chars.Length)];
        tmpStr += chars[Random.Shared.Next(chars.Length)];
        tmpStr += chars[Random.Shared.Next(chars.Length)];
        
        players[players.Count - 1].appearance = tmpStr;
        **/
    }


}
