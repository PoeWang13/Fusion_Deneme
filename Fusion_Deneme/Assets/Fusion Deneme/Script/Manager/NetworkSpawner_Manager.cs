using Fusion;
using System;
using UnityEngine;
using Fusion.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class NetworkSpawner_Manager : SimulationBehaviour, INetworkRunnerCallbacks
{
    public static NetworkRunner LocalRunner;

    private static NetworkSpawner_Manager instance;
    public static NetworkSpawner_Manager Instance {  get { return instance; } }
    // Public or inspector values
    [SerializeField] private NegentraPlayer playerPrefab;
    [SerializeField] private int foodAmount = 250;
    [SerializeField] private NetworkObject foodPrefab;

    // Private values
    private NegentraInput negentraInput;
    private bool creatingAllFood = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private NegentraReward reward;
    private NegentraReward rewarder;
    #region Genel
    public void SpawnAllFood()
    {
        GameObject allParent = GameObject.Find("All Parent");
        if (allParent == null)
        {
            allParent = new GameObject("All Parent");
        }
        GameObject foodParent = GameObject.Find("Food Parent");
        if (foodParent == null)
        {
            foodParent = new GameObject("Food Parent");
        }
        Transform foodParentParent = foodParent.transform;
        foodParent.transform.SetParent(allParent.transform);
        for (int i = 0; i < foodAmount; i++)
        {
            Vector3 foodPos = Utils.LearnRandomSpawnPoint(50);
            NetworkObject negentraFood = Runner.Spawn(foodPrefab, foodPos, Quaternion.identity);
            negentraFood.transform.position = foodPos;
            negentraFood.transform.SetParent(foodParentParent);
        }
        creatingAllFood = true;
    }
    #endregion

    public void GameFinish(string size, string kill)
    {
        rewarder.RPC_GameFinish(size, kill);
    }
    public void ExitSession()
    {
        _ = ShutdownRunner();
    }
    private async Task ShutdownRunner()
    {
        await LocalRunner?.Shutdown(destroyGameObject: false);
    }
    #region Fusion
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            //Warning_Manager.Instance.ShowMessage("On Player Joined.", 5);
            if (!creatingAllFood)
            {
                SpawnAllFood();

                rewarder = runner.Spawn(reward, transform.position, Quaternion.identity, player);
                rewarder.GameStart();
            }
            else
            {
                rewarder = FindObjectOfType<NegentraReward>();
            }
            NegentraPlayer negentraPlayer = runner.Spawn(playerPrefab, Utils.LearnRandomSpawnPoint(50), Quaternion.identity, player);
            negentraPlayer.playerState = PlayerState.Connection;
        }

        if (runner.LocalPlayer == player)
        {
            LocalRunner = runner;
        }
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //Utils.DebugLog("On Session List Updated.");
        Canvas_Manager_Game.Instance.SetUpdateLobbyList(sessionList);
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (negentraInput == null)
        {
            if (NegentraPlayer.Local == null)
            {
                return;
            }
            negentraInput = NegentraPlayer.Local.GetComponent<NegentraInput>();
        }
        else if (negentraInput != null)
        {
            input.Set(negentraInput.GetNetworkInputData());
        }
    }
    #endregion

    #region interface
    public void OnConnectedToServer(NetworkRunner runner)
    {
        //Warning_Manager.Instance.ShowMessage("On Connected To Server.", 2);
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        rewarder.RemoveNegetraPlayer();
        NegentraPlayer data = GetPlayerData(player, runner);
        runner.Despawn(data.Object);
        runner.Despawn(runner.GetPlayerObject(player));
        //Warning_Manager.Instance.ShowMessage("On Player Left.", 2);
    }
    private NegentraPlayer GetPlayerData(PlayerRef player, NetworkRunner runner)
    {
        NetworkObject NO;
        if (runner.TryGetPlayerObject(player, out NO))
        {
            NegentraPlayer data = NO.GetComponent<NegentraPlayer>();
            return data;
        }
        else
        {
            Debug.LogWarning("Player not found");
            return null;
        }
    }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        //Warning_Manager.Instance.ShowMessage("On Shutdown.", 2);
        creatingAllFood = false;
        Game_Manager.Instance.RestartMenu();
    }
    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        //Warning_Manager.Instance.ShowMessage("On Disconnected From Server.", 2);
    }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        //Warning_Manager.Instance.ShowMessage("On Connect Request.", 2);
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        //Warning_Manager.Instance.ShowMessage("On Connect Failed.", 2);
    }
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Utils.DebugLog("On Scene Load Done.");
    }
    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Utils.DebugLog("On Scene Load Start.");
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }
    #endregion
}