using TMPro;
using Fusion;
using UnityEngine;
using System.Text;
using System.Collections.Generic;

public class Canvas_Manager_Game : Singletion<Canvas_Manager_Game>
{
    public enum CanvasType { record, lobby, logIn }

    // Public or inspector values
    [Header("Genel")]
    [SerializeField] private GameObject objPanelOnline;
    [SerializeField] private String_Value valuePlayerName;

    [Header("Chat")]
    [SerializeField] private GameObject objChat;
    [SerializeField] private Animator aniChat;
    [SerializeField] private TextMeshProUGUI textChat;
    [SerializeField] private TMP_InputField inputChat;

    [Header("Lobby Record")]
    [SerializeField] private GameObject objLobbyData;
    [SerializeField] private TextMeshProUGUI textLobbySize;
    [SerializeField] private TextMeshProUGUI textLobbyKill;
    [SerializeField] private TextMeshProUGUI textLobbyDead;

    [Header("Player Data")]
    [SerializeField] private GameObject objPlayerData;
    [SerializeField] private TextMeshProUGUI textPlayerSize;
    [SerializeField] private TextMeshProUGUI textPlayerKill;
    [SerializeField] private TextMeshProUGUI textPlayerDead;

    [Header("Lobby")]
    [SerializeField] private GameObject objLobbyRoot;
    [SerializeField] private Transform objLobbyParent;
    [SerializeField] private Lobby objLobbyPrefab;
    [SerializeField] private GameObject objLobbyName;
    [SerializeField] private TextMeshProUGUI textLobbyName;
    [SerializeField] private TMP_InputField inputSessionName;
    [SerializeField] private TMP_InputField inputSessionPassword;

    [Header("Game Finish")]
    [SerializeField] private GameObject objGameFinish;
    [SerializeField] private GameObject objGameTime;
    [SerializeField] private TextMeshProUGUI textGameTime;
    [SerializeField] private TextMeshProUGUI textSizeRecorder;
    [SerializeField] private TextMeshProUGUI textKillRecorder;

    public void GameTime(int s)
    {
        textGameTime.text = "Game Time : " + s;
    }
    public void GameFinish(string size, string kill)
    {
        objGameFinish.SetActive(true);
        textSizeRecorder.text = "Size Recorder : " + size;
        textKillRecorder.text = "Kill Recorder: " + kill;
    }
    // Canvasda Panel-Game Finish panelindeki butona verildi.
    public void ReturnLobby()
    {
        NetworkSpawner_Manager.Instance.ExitSession();
    }
    // Private values
    private NegentraChat fusionChatSystem;
    private StringBuilder sb = new StringBuilder();

    #region Genel
    public void OnlineConnection(bool isActive)
    {
        objPanelOnline.SetActive(!isActive);
    }
    // Canvasda Exit butonuna atandı.
    public void Exit()
    {
        Save_Load_Manager.Instance.SaveGame();
        Application.Quit();
    }
    #endregion

    #region Data
    public void SetTextLobbySize(string playerName, int playerSize)
    {
        textLobbySize.text = playerName + " -> Size : " + playerSize.ToString();
    }
    public void SetTextLobbyKill(string playerName, int totalKill)
    {
        textLobbyKill.text = playerName + " -> Kill : " + totalKill.ToString();
    }
    public void SetTextLobbyDead(string playerName, int totalDead)
    {
        textLobbyDead.text = playerName + " -> Dead : " + totalDead.ToString();
    }
    public void SetTextPlayerSize(int playerSize)
    {
        textPlayerSize.text = "Size : " + playerSize.ToString();
    }
    public void SetTextPlayerKill(int totalKill)
    {
        textPlayerKill.text = "Kill : " + totalKill.ToString();
    }
    public void SetTextPlayerDead(int totalDead)
    {
        textPlayerDead.text = "Dead : " + totalDead.ToString();
    }
    #endregion

    #region Chat
    public void Chat(string chat)
    {
        sb.Length = 0;
        sb.AppendLine();
        sb.Append(chat);
        textChat.text += sb.ToString();
    }
    public void SetLocalChatPlayer(NegentraChat fusionChat)
    {
        fusionChatSystem = fusionChat;
    }
    // Canvasda Chat message yazma inputuna atandı.
    public void SendChat()
    {
        sb.Length = 0;
        sb.Append("<color=red>" + NegentraPlayer.Local.playerName.ToString() + " : </color>");
        sb.AppendLine();
        sb.Append(inputChat.text);
        fusionChatSystem.RPC_Chat(sb.ToString());
        inputChat.text = "";
    }
    // Canvasda Chat panelini açma-kapama butonuna atandı.
    public void OpenCloseChatArea()
    {
        aniChat.SetBool("Chat", !aniChat.GetBool("Chat"));
    }
    #endregion

    #region Lobby
    // Canvasda Record-Root panelinde Create Player butonuna verildi.
    public void CreateLobby()
    {
        if (string.IsNullOrEmpty(inputSessionName.text))
        {
            Warning_Manager.Instance.ShowMessage("You can't record empty session name.");
            return;
        }
        objLobbyRoot.SetActive(false);
        NetworkRunnerManager.Instance.CreateSession(inputSessionName.text, inputSessionPassword.text);
    }
    public void JoinGame()
    {
        objChat.SetActive(true);
        objPlayerData.SetActive(true);
        objLobbyData.SetActive(true);
        objGameTime.SetActive(true);
        if (NegentraPlayer.Local != null)
        {
            DestroyLobbyList();
            NegentraPlayer.Local.PlayerJoinedGame(valuePlayerName.GetStringValue());
            objLobbyName.SetActive(true);
            textLobbyName.text = "Lobby Name : " + inputSessionName.text;
        }
    }
    public void CloseLobbyPanel(string sessionName)
    {
        DestroyLobbyList();
        objLobbyRoot.SetActive(false);
        objLobbyName.SetActive(true);
        textLobbyName.text = "Lobby Name : " + sessionName;
    }
    public void SetUpdateLobbyList(List<SessionInfo> allSessions)
    {
        DestroyLobbyList();
        foreach (SessionInfo session in allSessions)
        {
            if (session.IsVisible && session.IsValid)
            {
                Lobby lobby = Instantiate(objLobbyPrefab, objLobbyParent);
                lobby.SetPlayerCount(session);
            }
        }
    }
    private void DestroyLobbyList()
    {
        foreach (Transform lobby in objLobbyParent)
        {
            Destroy(lobby.gameObject);
        }
    }
    #endregion
}