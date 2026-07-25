using Microsoft.Extensions.DependencyInjection;
using System;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class ConnectToServerMono : MonoBehaviour
{
    private ConnectionService _connectionService;

    private Action _onConnectCallback;
    private bool _hasCallback = false;

    public void TryToConnect()
    {
        _connectionService.InitiateConnection(_onConnectCallback);
    }

    private void Awake()
    {
        _connectionService = ServiceRegistration.ServiceProvider.GetRequiredService<ConnectionService>();
        DontDestroyOnLoad(this);
        _onConnectCallback += () =>
        {
            Debug.Log("Connection Callback");
            _hasCallback = true;
        };

        TryToConnect();
    }

    private void Update()
    {
        _connectionService.Connection.FrameTick();

        if (!_hasCallback)
            return;

        if (_connectionService.Connection == null)
        {
            Debug.Log("LostConnection");
            _hasCallback = false;
        }
    }
}
