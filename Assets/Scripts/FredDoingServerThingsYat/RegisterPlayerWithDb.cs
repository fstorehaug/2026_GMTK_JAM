using System;
using SpacetimeDB;
using SpacetimeDB.Types;


public class RegisterPlayerWithDb
{
    private ConnectionService _connection;

    public RegisterPlayerWithDb(ConnectionService connection)
    {
        _connection = connection;
    }

    public Action regitratinSucess;
    public Action registrationFail;

    public void RegisterPlayer(string playerName)
    {
        _connection.Connection.Reducers.OnCreateOrLoadPlayer += (ctx, name) =>
        {
            if (ctx.Event.Status is Status.Failed)
            {
                registrationFail?.Invoke();
            }

            regitratinSucess.Invoke();
        };

        _connection.Connection.Reducers.CreateOrLoadPlayer(playerName);
    }


}

public class ConnectionService
{
    string host = "http://127.0.0.1:3000";
    string dbName = "snailtrail";

    public DbConnection Connection { get; private set; }

    public ConnectionService()
    {
         DbConnection.Builder().WithUri(host)
            .WithDatabaseName("your_database_name")
            .OnConnect((conn, identity, token) =>
            {
                Connection = conn;
            }).Build();
    }
}