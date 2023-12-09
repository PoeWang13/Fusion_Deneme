using Fusion;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class NetworkRunnerManager : MonoBehaviour
{
    private static NetworkRunnerManager instance;
    public static NetworkRunnerManager Instance { get { return instance; } }

    // Public or inspector values
    [SerializeField] private NetworkRunner networkRunnerPrefab;

    // Private values
    private NetworkRunner networkRunner;
    public NetworkRunner NetworkRunner { get { return networkRunner; } }

    #region Genel
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
        networkRunner = FindObjectOfType<NetworkRunner>();
        if (networkRunner == null)
        {
            networkRunner = Instantiate(networkRunnerPrefab);
        }
        networkRunner.ProvideInput = true;
        ConnectLobby();
    }
    public async void ConnectLobby()
    {
        await networkRunner.JoinSessionLobby(SessionLobby.Shared);
    }
    public async void CreateSession(string sessionName, string sessionKey)
    {
        Dictionary<string, SessionProperty> sessionProperty = new Dictionary<string, SessionProperty>();
        sessionProperty.Add("NegentraKey", sessionKey);
        if (networkRunner == null)
        {
            networkRunner = FindObjectOfType<NetworkRunner>();
        }
        NetworkSceneManagerDefault networkSceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>();
        if (networkSceneManager == null)
        {
            networkSceneManager = networkRunner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }
        await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.AutoHostOrClient,
            PlayerCount = 2,
            SessionName = sessionName,
            SessionProperties = sessionProperty,
            SceneManager = networkSceneManager
        });
        Canvas_Manager_Game.Instance.JoinGame();
    }
    public async void ConnectSession(string sessionName)
    {
        if (networkRunner == null)
        {
            networkRunner = FindObjectOfType<NetworkRunner>();
        }
        NetworkSceneManagerDefault networkSceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>();
        if (networkSceneManager == null)
        {
            networkSceneManager = networkRunner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }
        await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Client,
            SessionName = sessionName,
            SceneManager = networkSceneManager
        });
        Canvas_Manager_Game.Instance.JoinGame();
    }
    #endregion
}