using Fusion;
using UnityEngine;

public class NegentraWeapon : NetworkBehaviour
{
    // Private values
    private NegentraStats negentraStats;
    private NegentraMovement negentraMovement;
    private Transform view;
    // Missile
    public GameObject minePrefab;
    private float lastTimeMine;
    private float lastTimeMineNext = 1.0f;
    // Rocket
    public GameObject rocketPrefab;
    private float lastTimeRocket;
    private float lastTimeRocketNext = 2.5f;

    #region Genel
    private void SendMine(bool canMine)
    {
        if (Time.time - lastTimeMine < lastTimeMineNext)
        {
            return;
        }
        if (!canMine)
        {
            return;
        }
        if (!negentraStats.CanCreateMine())
        {
            return;
        }
        negentraStats.CreateMine();
        Runner.Spawn(minePrefab, transform.position, Quaternion.identity, Object.InputAuthority, (runner, spawnedMine) =>
        {
            spawnedMine.GetComponent<NegentraMine>().SetMine(view.localScale.x);
        });
        lastTimeMine = Time.time;
    }
    private void SendRocket(bool canRocket)
    {
        if (Time.time - lastTimeRocket < lastTimeRocketNext)
        {
            return;
        }
        if (!canRocket)
        {
            return;
        }
        if (!negentraStats.CanCreateRocket())
        {
            return;
        }
        negentraStats.CreateRocket();
        Runner.Spawn(rocketPrefab, transform.position, Quaternion.identity, Object.InputAuthority, (runner, spawnedRocket) =>
        {
            spawnedRocket.GetComponent<NegentraRocket>().SetRocket(view.localScale.x, negentraMovement.Input);
        });
        lastTimeRocket = Time.time;
    }
    #endregion

    #region Fusion
    public override void Spawned()
    {
        base.Spawned();
        negentraStats = GetComponent<NegentraStats>();
        negentraMovement = GetComponent<NegentraMovement>();
        view = transform.Find("View");
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            SendMine(networkInputData.canMine);
            SendRocket(networkInputData.canRocket);
        }
    }
    #endregion
}