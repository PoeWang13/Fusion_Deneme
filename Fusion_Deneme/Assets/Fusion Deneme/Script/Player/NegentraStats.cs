using Fusion;
using UnityEngine;

public class NegentraStats : NetworkBehaviour
{
    // Network Values
    [Networked(OnChanged = nameof(OnSizeChanged))]
    public ushort playerSize { get; set; }
    [Networked(OnChanged = nameof(OnKillChanged))]
    public int playerKill { get; set; }
    [Networked(OnChanged = nameof(OnDeadChanged))]
    public int playerDead { get; set; }

    // Private values
    private Transform view;
    private Camera myCamera;
    private Rigidbody2D myRigid;
    private NegentraPlayer negentraPlayer;
    private NegentraMovement negentraMovement;
    private NegentraReward negentraReward;
    public string playerName;

    // Properties
    public string PlayerName { get { return playerName; } }
    public NegentraPlayer NegentraPlayer { get { return negentraPlayer; } }

    #region Networked stat fonksiyons
    protected static void OnSizeChanged(Changed<NegentraStats> changed)
    {
        changed.Behaviour.UpdateSize();
    }
    protected static void OnKillChanged(Changed<NegentraStats> changed)
    {
        changed.Behaviour.UpdateKill();
    }
    protected static void OnDeadChanged(Changed<NegentraStats> changed)
    {
        changed.Behaviour.UpdateDead();
    }
    private void UpdateKill()
    {
        if (Object.HasInputAuthority)
        {
            Canvas_Manager_Game.Instance.SetTextPlayerKill(playerKill);
        }
        negentraReward.LobbyKillRecord();
    }
    public void IncreaseKill()
    {
        playerKill++;
    }
    private void UpdateDead()
    {
        if (Object.HasInputAuthority)
        {
            Canvas_Manager_Game.Instance.SetTextPlayerDead(playerDead);
        }
        negentraReward.LobbyDeadRecord();
    }
    public void IncreaseDead()
    {
        playerDead++;
    }
    #endregion

    #region Fusion
    public override void Spawned()
    {
        base.Spawned();
        myCamera = Camera.main;
        negentraPlayer = GetComponent<NegentraPlayer>();
        negentraMovement = GetComponent<NegentraMovement>();
        negentraReward = FindObjectOfType<NegentraReward>();
        negentraReward.AddNegetraPlayer(this);
        myRigid = GetComponent<Rigidbody2D>();
        view = transform.Find("View");

        CollectedFood(5);
        UpdateSize();
    }
    #endregion

    #region Data
    public void SetPlayerSizeRecord()
    {
        if (!string.IsNullOrEmpty(playerName))
        {
            RPC_SendToEveryPlayer_PlayerSizeRecord(playerName);
        }
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SendToEveryPlayer_PlayerSizeRecord(string nickName)
    {
        Canvas_Manager_Game.Instance.SetTextLobbySize(nickName, playerSize);
    }
    public void SetPlayerKillRecord()
    {
        if (!string.IsNullOrEmpty(playerName))
        {
            RPC_SendToEveryPlayer_PlayerKillRecord(playerName);
        }
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SendToEveryPlayer_PlayerKillRecord(string nickName)
    {
        Canvas_Manager_Game.Instance.SetTextLobbyKill(nickName, playerKill);
    }
    public void SetPlayerDeadRecord()
    {
        if (!string.IsNullOrEmpty(playerName))
        {
            RPC_SendToEveryPlayer_PlayerDeadRecord(playerName);
        }
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SendToEveryPlayer_PlayerDeadRecord(string nickName)
    {
        Canvas_Manager_Game.Instance.SetTextLobbyDead(nickName, playerDead);
    }
    #endregion

    #region Genel
    private void LateUpdate()
    {
        if (Object.HasInputAuthority)
        {
            Fooded();
        }
    }
    public void SetPlayerName(string named)
    {
        playerName = named;
        negentraReward.SendLobbyRecords();
    }
    // Food functions
    public void CollectedFood(ushort growSize)
    {
        playerSize += growSize;
        UpdateSize();
        negentraReward.LobbySizeRecord();
    }
    public void UsedFoodEnergy(ushort growSize)
    {
        if (playerSize <= growSize)
        {
            playerSize = 5;
            negentraPlayer.PlayerDead();
            myRigid.velocity = Vector2.zero;
        }
        else
        {
            playerSize -= growSize;
        }
        UpdateSize();
        negentraReward.LobbySizeRecord();
    }
    // Birþeyler yediðinde kameranýn görüþ açýsýný büyütüp küçültmeye yarýyor.
    // Do tween eklendiðinde ona göre upgrade edilecek.
    public void Fooded()
    {
        float orthSize = (view.localScale.x + 7) / myCamera.aspect;
        myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, orthSize, Time.deltaTime * 0.1f);
    }
    private void UpdateSize()
    {
        view.localScale = Vector3.one + Vector3.one * 100 * (playerSize / 65535.0f);
        negentraMovement.SetPlayerSize(playerSize);
        if (Object.HasInputAuthority)
        {
            Canvas_Manager_Game.Instance.SetTextPlayerSize(playerSize);
        }
    }
    public void ResetPlayerSize()
    {
        playerSize = 5;
        UpdateSize();
    }
    // Rocket functions
    public bool CanCreateRocket()
    {
        return playerSize > 25;
    }
    public void CreateRocket()
    {
        UsedFoodEnergy(25);
    }
    // Mine functions
    public bool CanCreateMine()
    {
        return playerSize > 10;
    }
    public void CreateMine()
    {
        UsedFoodEnergy(10);
    }
    #endregion
}