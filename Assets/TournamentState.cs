using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;


public class TournamentState:MonoBehaviour
{

    public List<TournamentPlayer> players;
    // Static reference readable by any script, but writable only by this class
    public static TournamentState Instance { get; private set; }

    private void Awake()
    {
        // Enforce the Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Optional: Keeps the manager alive when changing scenes
        DontDestroyOnLoad(gameObject);
    }

    // Example global method
    public void UpdateScore(int amount)
    {
        Debug.Log($"Score updated by {amount}");
    }

    public void AddPlayer(string name)
    {
        players.Add(new TournamentPlayer());
        players[players.Count - 1].name = name;

        /** appearance code
        string tmpStr = "";
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        tmpStr += chars[Random.Shared.Next(chars.Length)];
        tmpStr += chars[Random.Shared.Next(chars.Length)];
        tmpStr += chars[Random.Shared.Next(chars.Length)];
        
        players[players.Count - 1].appearance = tmpStr;
        **/
    }
    public void updatePlayerScore (int playerNumber, float addedScore)
    {
        players[playerNumber].totalScore += addedScore;
    }

    public void incrementPlayerShot(int playerNumber)
    {
        players[playerNumber].timesShot += 1;
    }

    public void updatePlayerAppearance(int playerNumber, string appearance)
    {
        players[playerNumber].appearance =appearance;
    }
}