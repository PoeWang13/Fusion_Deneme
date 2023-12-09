using Fusion;
using UnityEngine;
using System.Collections.Generic;

public class NegentraReward : NetworkBehaviour
{
    #region OnGameTimeChanged
    [Networked(OnChanged = nameof(OnGameTimeChanged))]
    public int gameTime { get; set; }
    public float gameTimeNext;
    public bool gameStart;
    private void Update()
    {
        if (gameStart)
        {
            gameTimeNext += Time.deltaTime;
            if (gameTimeNext > 1)
            {
                gameTimeNext--;
                gameTime--;
                if (gameTime == 0)
                {
                    gameStart = false;
                    StopNegentraPlayer();
                }
            }
        }
    }
    protected static void OnGameTimeChanged(Changed<NegentraReward> changed)
    {
        changed.Behaviour.GameTimeChanged();
    }
    public void GameTimeChanged()
    {
        Canvas_Manager_Game.Instance.GameTime(gameTime);
    }
    public void GameStart()
    {
        gameStart = true;
        gameTime = 15;
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_GameFinish(string size, string kill)
    {
        Canvas_Manager_Game.Instance.GameFinish(size, kill);
    }
    #endregion

    // Private
    private List<NegentraStats> negentralar = new List<NegentraStats>();
    public void AddNegetraPlayer(NegentraStats negetra)
    {
        negentralar.Add(negetra);
    }
    public void RemoveNegetraPlayer()
    {
        for (int e = negentralar.Count - 1; e >= 0; e--)
        {
            negentralar.RemoveAt(e);
        }
    }
    public void StopNegentraPlayer()
    {
        NegentraStats negentraSizeRecorder = negentralar[0];
        NegentraStats negentraKillRecorder = negentralar[0];
        for (int e = 0; e < negentralar.Count; e++)
        {
            negentralar[e].NegentraPlayer.playerState = PlayerState.GameFinish;

            if (negentralar[e].playerSize > negentraSizeRecorder.playerSize)
            {
                negentraSizeRecorder = negentralar[e];
            }
            if (negentralar[e].playerKill > negentraKillRecorder.playerKill)
            {
                negentraKillRecorder = negentralar[e];
            }
        }

        NetworkSpawner_Manager.Instance.GameFinish(negentraSizeRecorder.PlayerName, negentraKillRecorder.PlayerName);
    }
    public void LobbySizeRecord()
    {
        NegentraStats negentra = negentralar[0];

        for (int e = 1; e < negentralar.Count; e++)
        {
            if (negentralar[e].playerSize > negentra.playerSize)
            {
                negentra = negentralar[e];
            }
        }
        negentra.SetPlayerSizeRecord();
    }
    public void LobbyKillRecord()
    {
        NegentraStats negentra = negentralar[0];

        for (int e = 1; e < negentralar.Count; e++)
        {
            if (negentralar[e].playerKill > negentra.playerKill)
            {
                negentra = negentralar[e];
            }
        }
        negentra.SetPlayerKillRecord();
    }
    public void LobbyDeadRecord()
    {
        NegentraStats negentra = negentralar[0];

        for (int e = 1; e < negentralar.Count; e++)
        {
            if (negentralar[e].playerDead < negentra.playerDead)
            {
                negentra = negentralar[e];
            }
        }
        negentra.SetPlayerDeadRecord();
    }
    public void SendLobbyRecords()
    {
        LobbySizeRecord();
        LobbyKillRecord();
        LobbyDeadRecord();
    }
}