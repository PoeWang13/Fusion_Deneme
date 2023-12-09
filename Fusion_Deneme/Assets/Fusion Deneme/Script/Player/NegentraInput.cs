using Fusion;
using UnityEngine;

public class NegentraInput : NetworkBehaviour
{
    // Private values
    private bool canMine;
    private bool canRocket;
    private Vector2 mouseDirection = Vector2.zero;

    #region Genel
    private void Update()
    {
        mouseDirection = (Utils.LearnMouseWorldPosition() - transform.position).normalized;

        if (Input.GetKeyDown(KeyCode.B))
        {
            canRocket = true;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            canMine = true;
        }
    }
    public NetworkInputData GetNetworkInputData()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        networkInputData.inputMovement = mouseDirection;
        networkInputData.canMine = canMine;
        networkInputData.canRocket = canRocket;

        canMine = false;
        canRocket = false;

        return networkInputData;
    }
    #endregion
}