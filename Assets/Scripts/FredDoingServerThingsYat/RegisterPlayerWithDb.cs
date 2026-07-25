using SpacetimeDB;
using SpacetimeDB.Types;
using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.MemoryProfiler;
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

    const string AuthTokenKey = "SpacetimeDB.AuthToken";

    public ConnectionService()
    {
      
    }

    public void InitiateConnection(Action callback)
    {
        var token = PlayerPrefs.GetString(AuthTokenKey, null);

        var connectionBuilder = DbConnection.Builder()
            .WithUri(host)
            .WithDatabaseName(dbName);

        if (!string.IsNullOrEmpty(token))
        {
            connectionBuilder.WithToken(token);
        }

        connectionBuilder.OnConnect((conn, identity, token) =>
        {
            PlayerPrefs.SetString(AuthTokenKey, token);
            PlayerPrefs.Save();
            callback?.Invoke();
            Connection.SubscriptionBuilder().SubscribeToAllTables();

        });

        Connection = connectionBuilder
            .OnConnectError(x => Debug.Log($"failed to connect: {x.Message}"))
            .OnDisconnect((x, y) => Debug.Log($"Disconnected: {y?.Message}"))
            .Build();
    }
}