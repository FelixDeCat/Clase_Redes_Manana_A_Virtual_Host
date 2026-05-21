using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

public class PlayerSpawner : NetworkBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPrefabRef player_model;
    [SerializeField] Transform[] spawnPoints;

    Dictionary<PlayerRef, NetworkObject> players = new Dictionary<PlayerRef, NetworkObject>();

    public override void Spawned()
    {
        //if (!Runner.IsServer) return;
        Runner.AddCallbacks(this); // Todos tienen que escuchar
    }
    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)  // servers
    {
        if (!Runner.IsServer) return;
        Transform point = spawnPoints[runner.SessionInfo.PlayerCount - 1];
        var obj = runner.Spawn(player_model, point.position, point.rotation, player);
        if (!players.ContainsKey(player))
        {
            players.Add(player, obj);
        }
    }
    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player) //servers
    {
        if (!Runner.IsServer) return;
        NetworkObject desconected;
        if (players.ContainsKey(player))
        {
            desconected = players[player];
            players.Remove(player);
        }
    }
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input) 
    {
        if (LocalInputs.instance) // en todos los que tengan la autoridad de Input (Singleton único para InputAuth)
        {
            NetworkPlayerInputData inputdata = LocalInputs.instance.GetInputData();
            input.Set(inputdata);
        }
    }







    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
}
