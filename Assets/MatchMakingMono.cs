using Microsoft.Extensions.DependencyInjection;
using SpacetimeDB.Types;
using UnityEngine;

public class MatchMakingMono : MonoBehaviour
{
    void Start()
    {
        var MatchMaking = ServiceRegistration.ServiceProvider.GetRequiredService<MatchMaking>();

        MatchMaking.onMatchMakingFailed += () => { Debug.Log("Rohrough, faar ikke laav aa spille"); };
        MatchMaking.onMatchMakingSuccess += OnMatchFound;
        MatchMaking.onMathcUpdate += OnMatchUpdated;
        MatchMaking.MakeaDaMactch();
    }

    public void OnMatchFound(Match match)
    {

    }

    public void OnMatchUpdated(Match match)
    {


    }

}
