using System;
using SpacetimeDB;
using SpacetimeDB.Types;
using UnityEngine;


public class RegisterPlayerWithDb
{
    public Action regitratinSucess;
    public Action registrationFail;

    private ConnectionService _connectionService;

    public RegisterPlayerWithDb(ConnectionService connectionService)
    {
        _connectionService = connectionService;
    }

    public void RegisterPlayer(string playerName)
    {
        _connectionService.Connection.Reducers.OnCreateOrLoadPlayer += (ctx, name) =>
        {
            if (ctx.Event.Status is Status.Failed)
            {
                registrationFail?.Invoke();
            }

            regitratinSucess.Invoke();
        };

        _connectionService.Connection.Reducers.CreateOrLoadPlayer(playerName);
    }
}

public class ConnectionService
{
    static string host = "https://maincloud.spacetimedb.com";
    static string dbName = "snailtrail";

    public  DbConnection Connection { get; private set; }
    public  Identity Identity { get; private set; }
    public  string Token { get; private set; }

    private DbConnection _con;
    public ConnectionService()
    {
      
    }

    public void InitiateConnection(Action callback)
    {
        Connection = DbConnection.Builder().WithUri(host)
            .WithDatabaseName(dbName)
            .OnConnectError(x => Debug.Log($"failed to connect: {x.Message}"))
            .OnDisconnect((x, y) => Debug.Log($"Disconnected: {y?.Message}"))
            .OnConnect((conn, identity, token) =>
            {
                _con = conn;
                Identity = identity;
                Token = token;
                Debug.Log("Connected");
                callback?.Invoke();
            }).Build();
    }
}