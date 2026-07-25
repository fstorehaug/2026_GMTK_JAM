using System;
using SpacetimeDB;
using UnityEngine;

public class ShootService
{
    private ConnectionService _connectionService;

    public Action OnShootSuccess;

    public ShootService(ConnectionService connectionService)
    {
        _connectionService = connectionService;
    }

    public void SHOOT(float timeInMiliseconds)
    {
        _connectionService.Connection.Reducers.OnShoot += Reducers_OnShoot;
        _connectionService.Connection.Reducers.Shoot(timeInMiliseconds);
    }

    private void Reducers_OnShoot(SpacetimeDB.Types.ReducerEventContext ctx, float timeInMilSeconds)
    {
        if (ctx.Event.Status is Status.Failed)
        {
            Debug.Log("Jaevelig daarlig aa ikke klare aa skyte i er spill om skyting");
        } 

        OnShootSuccess?.Invoke(); //TODO: do we need to do something, i think matchmaking does the rest
    }
}
