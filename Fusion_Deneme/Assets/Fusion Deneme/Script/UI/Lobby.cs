using TMPro;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Lobby : MonoBehaviour
{
    [SerializeField] private Button buttonJoin;
    [SerializeField] private TextMeshProUGUI textSessionName;
    [SerializeField] private TextMeshProUGUI textPlayerCount;
    [SerializeField] private TMP_InputField inputSessionPassword;
    private bool canJoin;
    private bool hasKey;
    private SessionInfo session;
    public void SetPlayerCount(SessionInfo session)
    {
        this.session = session;
        textSessionName.text = session.Name;
        textPlayerCount.text = session.PlayerCount + " / " + session.MaxPlayers;
        canJoin = session.PlayerCount != session.MaxPlayers;
        buttonJoin.interactable = canJoin;
        inputSessionPassword.interactable = canJoin;
        // Lobby'nin passwordu var.
        if (!string.IsNullOrEmpty(session.Properties.GetValueOrDefault("NegentraKey").PropertyValue.ToString()))
        {
            hasKey = true;
            if (session.PlayerCount == session.MaxPlayers)
            {
                return;
            }
            inputSessionPassword.gameObject.SetActive(true);
        }
    }
    // Lobby Prefabýnda Join butonuna atandý.
    public void JoinLobby()
    {
        if (!canJoin)
        {
            Warning_Manager.Instance.ShowMessage("This lobby has full.");
            return;
        }
        if (session.PlayerCount == session.MaxPlayers)
        {
            inputSessionPassword.interactable = false;
            buttonJoin.interactable = false;
            Warning_Manager.Instance.ShowMessage("This lobby has full.");
            return;
        }
        if (!session.IsValid)
        {
            Warning_Manager.Instance.ShowMessage("This lobby has not valid.");
            return;
        }
        if (hasKey)
        {
            string key = session.Properties.GetValueOrDefault("NegentraKey").PropertyValue.ToString();
            if (key != inputSessionPassword.text)
            {
                Warning_Manager.Instance.ShowMessage("You enter Invalid Password.");
                return;
            }
        }
        Canvas_Manager_Game.Instance.CloseLobbyPanel(session.Name);
        NetworkRunnerManager.Instance.ConnectSession(session.Name);
    }
}