using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class RunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkRunner _runnerPrefab;
    NetworkRunner _runner;

    [SerializeField] NetworkSceneManagerDefault _sceneManager;

    [SerializeField] string lobbyID = "GenericLobby";
    [SerializeField] string roomID = "GenericRoom";

    void Start()
    {
        //NetworkUI.instance.BeginLoad();
        //NetworkUI.instance.SubscribeToEvents(HostOnCreateSession,BTN_Client);
    }

    Action _onConnectedToLobby = delegate { };
    Action<string> onFinishConnectToSession = delegate { }; /// host y client

    /// Igual para todos (Host-CLientes // Shared)
    public void JoinLobby(string _LobbyName, Action OnConnectedToLobby)
    {
        _onConnectedToLobby = OnConnectedToLobby;
        if (_runner)
        {
            Destroy(_runner.gameObject);
        }
        _runner = Instantiate(_runnerPrefab);
        _sceneManager = _runner.GetComponent<NetworkSceneManagerDefault>();

        _runner.AddCallbacks(this);

        JoinSessionLobby(_LobbyName);
    }

    async void JoinSessionLobby(string _LobbyName)
    {
        
        var result = await _runner.JoinSessionLobby(SessionLobby.Custom, _LobbyName);

        if (result.Ok)
        {
            print("<color=green>Todo OK!</color> nos Unimos a: " + _LobbyName);
        }
        else
        {
            print($"<color=red>Error:{result.ErrorMessage}</color>");
        }

        //_sceneManager.LoadScene();
        _onConnectedToLobby.Invoke();
       
    }

    async void CreateGame(GameMode gameMode, string sessionName, int sceneIndex)
    {
        _runner.ProvideInput = true;

        var result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            Scene = SceneRef.FromIndex(sceneIndex),
            SessionName = sessionName,
            SceneManager = _sceneManager,
           
        });

        if (result.Ok)
        {
            print("<color=green>Todo OK!</color>");

           
        }
        else
        {
            print($"<color=red>Error:{result.ErrorMessage}</color>");
        }

        // Esto lo borro porque se supone que el StartGame ya me carga una escena
        //onFinishConnectToSession.Invoke(result.Ok ? "todo ok": $"Error:{ result.ErrorMessage}");
    }


    public void HostOnCreateSession(string sessionName, int sceneIndex = 1, Action<string> _onFinishCreateSession = null)
    {
        this.onFinishConnectToSession = _onFinishCreateSession;

        CreateGame(GameMode.Host, sessionName, sceneIndex);
    }
    public void ClientConnectedToSession(string sessionName, int sceneIndex = 1, Action<string> onFinishJoinSessionGame = null)
    {
        this.onFinishConnectToSession = onFinishJoinSessionGame;
        CreateGame(GameMode.Client, sessionName, 1);
    }


    public void SubscribeToSessionLisUpdate(Action<List<SessionInfo>> _callback)
    {
        onListUpdated = _callback;
    }
    Action<List<SessionInfo>> onListUpdated = delegate { };
    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) 
    {
        onListUpdated.Invoke(sessionList);
    }
    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input) { }
    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner) { }
}
