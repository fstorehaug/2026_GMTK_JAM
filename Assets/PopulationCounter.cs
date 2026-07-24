using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulationCounter : MonoBehaviour
{
    [HideInInspector]
    public int Population = 1000000;
    [HideInInspector]
    public int saved = 0;

    [HideInInspector] public int costToTurn = 500;
    [HideInInspector] public int costToMove = 100;
    [HideInInspector] public int costToRandomizeBoard = 1000;

    [SerializeField] private TextMeshProUGUI populationText;
    [SerializeField] private TextMeshProUGUI savedText;

    public bool PayToRotate()
    {
        if (costToTurn > Population)
            return false;

        Population -= costToTurn;
        populationText.text = Population.ToString();

        return true;
    }

    public bool PayToMove()
    {
        if (costToMove > Population)
            return false;

        Population -= costToMove;
        populationText.text = Population.ToString();

        return true;
    }

    public bool PayToRandomizeBoard()
    {
        if (costToRandomizeBoard > Population)
            return false;

        Population -= costToRandomizeBoard;
        populationText.text = Population.ToString();

        return true;
    }

    private int LargeRocket = 5000;
    private int smalRocket = 500;


    public void SavePeople(Effect effect)
    {
        var toSave = 0;
        switch (effect)
        {
            case Effect.LargeRocket:
                toSave = LargeRocket;
                break;
            case Effect.SmallRocket:
                toSave = smalRocket;
                break;
        }

        toSave = Population < toSave ? Population : toSave;

        Population -= toSave;
        saved += toSave;
        populationText.text = Population.ToString();
        savedText.text = saved.ToString();
    }


}
