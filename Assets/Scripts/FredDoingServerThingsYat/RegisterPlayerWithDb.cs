using System;
using SpacetimeDB;
using SpacetimeDB.Types;


public class RegisterPlayerWithDb
{
    public RegisterPlayerWithDb()
    {
    }

    public Action regitratinSucess;
    public Action registrationFail;

    public void RegisterPlayer(string playerName)
    {
        ConnectionService.Connection.Reducers.OnCreateOrLoadPlayer += (ctx, name) =>
        {
            if (ctx.Event.Status is Status.Failed)
            {
                registrationFail?.Invoke();
            }

            regitratinSucess.Invoke();
        };

        ConnectionService.Connection.Reducers.CreateOrLoadPlayer(playerName);
    }


}

public static class ConnectionService
{
    static string host = "http://127.0.0.1:3000";
    static string dbName = "snailtrail";

    public static DbConnection Connection { get; private set; }
    public static Identity Identity { get; private set; }
    public static string Token { get; private set; }

    static ConnectionService()
    {
         DbConnection.Builder().WithUri(host)
            .WithDatabaseName("your_database_name")
            .OnConnect((conn, identity, token) =>
            {
                Connection = conn;
                Identity = identity;
                Token = token;
            }).Build();
    }
}