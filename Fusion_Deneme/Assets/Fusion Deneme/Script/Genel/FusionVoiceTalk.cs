using UnityEngine;
using Photon.Voice.Unity;

public class FusionVoiceTalk : MonoBehaviour
{
    // Public or inspector values
    [SerializeField] private Recorder recorder;

    #region Genel
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            recorder.TransmitEnabled = true;
            NegentraPlayer.Local.voiceState = true;
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            recorder.TransmitEnabled = false;
            NegentraPlayer.Local.voiceState = false;
        }
    }
    #endregion
}