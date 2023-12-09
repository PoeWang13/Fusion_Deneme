using Fusion;
using UnityEngine;

public class NegentraChat : NetworkBehaviour
{
    // Network Values
    [Networked(OnChanged = nameof(OnChatChanged))]
    public NetworkString<_128> LastChatMessage { get; set; }

    #region Networked stat fonksiyons
    protected static void OnChatChanged(Changed<NegentraChat> changed)
    {
        changed.Behaviour.LearnChat();
    }
    #endregion

    #region RPC
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Chat(string nickName, RpcInfo info = default)
    {
        LastChatMessage = nickName;
    }
    #endregion

    #region Fusion
    public override void Spawned()
    {
        base.Spawned();
        if (Object.HasInputAuthority)
        {
            Canvas_Manager_Game.Instance.SetLocalChatPlayer(this);
        }
    }
    #endregion

    #region Genel
    private void LearnChat()
    {
        Canvas_Manager_Game.Instance.Chat(LastChatMessage.ToString());
    }
    #endregion
}