using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Canvas_Manager_Preload : Singletion<Canvas_Manager_Preload>
{
    public enum CanvasType { record, lobby, logIn }

    // Public or inspector values
    [Header("Genel")]
    [SerializeField] private GameObject objPanelOnline;
    [SerializeField] private String_Value valuePlayerName;

    [Header("Record")]
    [SerializeField] private GameObject objRecord;
    [SerializeField] private GameObject objEnterPlayer;
    [SerializeField] private TMP_InputField inputPlayerName;
    [SerializeField] private TMP_Dropdown dropdownPlayerName;

    #region Genel
    private void Start()
    {
        if (Save_Load_Manager.Instance.gameData.allNames.Count > 0)
        {
            dropdownPlayerName.ClearOptions();

            dropdownPlayerName.AddOptions(Save_Load_Manager.Instance.gameData.allNames);
            valuePlayerName.SetStringValue(Save_Load_Manager.Instance.gameData.allNames[0].ToString());
        }
        else
        {
            objEnterPlayer.SetActive(false);
        }
    }
    // Canvasda Exit butonuna atandı.
    public void Exit()
    {
        Save_Load_Manager.Instance.SaveGame();
        Application.Quit();
    }
    #endregion

    #region Record
    // Canvasda Record-Root panelinde New Player butonuna verildi.
    public void RecordPlayer()
    {
        if (string.IsNullOrEmpty(inputPlayerName.text))
        {
            Warning_Manager.Instance.ShowMessage("You can't record empty player name.");
            return;
        }
        if (Save_Load_Manager.Instance.gameData.allNames.Contains(inputPlayerName.text))
        {
            Warning_Manager.Instance.ShowMessage("This player name already recorded.");
            return;
        }
        Save_Load_Manager.Instance.SavePlayer(inputPlayerName.text);
        valuePlayerName.SetStringValue(inputPlayerName.text);
        objRecord.SetActive(false);

        SceneManager.LoadScene("Game");
    }
    // Canvasda Record-Root panelinde Enter Player butonuna verildi.
    public void EnterPlayer()
    {
        objRecord.SetActive(false);
        SceneManager.LoadScene("Game");
    }
    // Canvasda Record-Root panelinde Player drop downuna verildi.
    public void SetPlayerName(int playerOrder)
    {
        valuePlayerName.SetStringValue(Save_Load_Manager.Instance.gameData.allNames[playerOrder].ToString());
    }
    #endregion
}