using TMPro;
using Fusion;
using UnityEngine;

public enum PlayerState { None, Connection, Playing, Dead, GameFinish }
public class NegentraPlayer : NetworkBehaviour
{
    public static NegentraPlayer Local { get; set; }

    // Public or inspector values
    [SerializeField] private TextMeshPro playerNameText;
    [SerializeField] private SpriteRenderer playerView;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private NegentraMovement playerMovement;
    [SerializeField] private NegentraStats negentraStats;
    [SerializeField] private NetworkRigidbody2D playerRigidbody;
    [SerializeField] private String_Value valuePlayerName;

    // Network Values
    [Networked(OnChanged = nameof(OnPlayerStateChanged))]
    public PlayerState playerState { get; set; }

    [Networked(OnChanged = nameof(OnNameChanged))]
    public NetworkString<_16> playerName { get; set; }

    [Networked(OnChanged = nameof(OnColorChanged))]
    private Color playerColor { get; set; }

    [Networked(OnChanged = nameof(OnVoiceChanged)), HideInInspector]
    public NetworkBool voiceState { get; set; }

    // Private Values
    private TickTimer deadTime = TickTimer.None;
    private TickTimer teleportTime = TickTimer.None;
    private GameObject voiceIcon;

    #region Genel
    private void SetPlayerView(bool canView)
    {
        playerView.gameObject.SetActive(canView);
        playerCollider.enabled = canView;
        playerMovement.enabled = canView;
    }
    private void ResetPlayer()
    {
        Vector3 newPosition = Utils.LearnRandomSpawnPoint();
        playerRigidbody.TeleportToPosition(newPosition);
        playerState = PlayerState.Playing;
        negentraStats.ResetPlayerSize();
    }
    [ContextMenu("Player Dead")]
    public void PlayerDead()
    {
        Utils.DebugLog($"Player {playerName} is dead.");
        playerState = PlayerState.Dead;
        deadTime = TickTimer.CreateFromSeconds(Runner, 5);
        teleportTime = TickTimer.CreateFromSeconds(Runner, 4.5f);
    }
    #endregion

    #region interface
    #endregion

    #region Fusion
    public override void Spawned()
    {
        Utils.DebugLog("A player spawned.");
        if (Object.HasInputAuthority)
        {
            Local = this;
            PlayerJoinedGame(valuePlayerName.GetStringValue());
        }
        if (Object.HasStateAuthority)
        {
            Color color = Utils.LearnRandomColor_0_1();
            playerColor = color;
        }
        voiceIcon = transform.Find("VoiceTalkingIcon").gameObject;
        Runner.SetPlayerObject(Object.InputAuthority, Object);

        gameObject.name = "Negentra Player - " + Object.Id;
    }
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            if (deadTime.Expired(Runner))
            {
                Utils.DebugLog($"Player {playerName} is life.");
                playerState = PlayerState.Playing;
                deadTime = TickTimer.None;
            }
            if (teleportTime.Expired(Runner))
            {
                ResetPlayer();
                teleportTime = TickTimer.None;
            }
        }
    }
    #endregion

    #region Networked stat fonksiyons
    // Name setting
    protected static void OnNameChanged(Changed<NegentraPlayer> changed)
    {
        changed.Behaviour.NameChanged();
    }
    private void NameChanged()
    {
        Utils.DebugLog($"We set {playerName}'s name.");
        playerNameText.text = playerName.ToString();
    }
    // Color setting
    protected static void OnColorChanged(Changed<NegentraPlayer> changed)
    {
        changed.Behaviour.playerView.color = changed.Behaviour.playerColor;
    }
    // State setting
    protected static void OnPlayerStateChanged(Changed<NegentraPlayer> changed)
    {
        changed.Behaviour.PlayerStateChangedChanged();
    }
    private void PlayerStateChangedChanged()
    {
        Utils.DebugLog($"{playerName}'s state changed.");
        if (playerState == PlayerState.Connection)
        {
            SetPlayerView(false);
        }
        else if (playerState == PlayerState.Playing)
        {
            if (Object.HasInputAuthority)
            {
                ResetPlayer();
                Camera.main.transform.position = transform.position;
            }
            SetPlayerView(true);
        }
        else if (playerState == PlayerState.Dead)
        {
            SetPlayerView(false);
            playerMovement.StopRigidVelocity();
            Warning_Manager.Instance.ShowMessage(playerName.ToString() + " is dead.");
            if (Object.HasInputAuthority)
            {
                Warning_Manager.Instance.ShowMessage("You are dead, loser.");
                //Save_Load_Manager.Instance.IncreaseDead();
            }
        }
        else if (playerState == PlayerState.GameFinish)
        {
            playerMovement.StopRigidVelocity();
            playerCollider.enabled = false;
            playerMovement.enabled = false;
        }
    }
    // Voice setting
    protected static void OnVoiceChanged(Changed<NegentraPlayer> changed)
    {
        changed.Behaviour.voiceIcon.SetActive(changed.Behaviour.voiceState);
    }
    #endregion

    #region RPC
    // Join setting
    public void PlayerJoinedGame(string playerName)
    {
        RPC_JoinGame(playerName);
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_JoinGame(string playerName)
    {
        Debug.Log($"{playerName} joined Game.");
        this.playerName = playerName;
        SetPlayerView(true);
        negentraStats.SetPlayerName(playerName);
        ResetPlayer();
    } 
    #endregion
}