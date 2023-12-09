using Fusion;
using UnityEngine;

public class NegentraMine : NetworkBehaviour
{
    // Public or inspector values
    [SerializeField] private GameObject mineEffectPrefab;
    [SerializeField] private LayerMask playerMask;

    // Private values
    private TickTimer activitedTime = TickTimer.None;
    private bool activited = false;
    private Transform view;

    #region Genel
    public void SetMine(float timer)
    {
        activitedTime = TickTimer.CreateFromSeconds(Runner, timer);
    }
    #endregion

    #region Fusion
    public override void Spawned()
    {
        base.Spawned();
        view = transform.Find("View");
    }
    public override void FixedUpdateNetwork()
    {
        if (activited)
        {
            Collider2D hitCollider = Runner.GetPhysicsScene2D().OverlapCircle(view.position, (view.localScale.x * 0.5f) * 0.8f, playerMask);
            if (hitCollider != null)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    NegentraStats negentraStats = hitCollider.GetComponent<NegentraStats>();
                    negentraStats.UsedFoodEnergy(25);
                    activited = false;
                    Runner.Despawn(Object);
                }
            }
        }
        else
        {
            if (Object.HasStateAuthority)
            {
                if (activitedTime.Expired(Runner))
                {
                    activitedTime = TickTimer.None;
                    activited = true;
                }
            }
        }
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        Destroy(Instantiate(mineEffectPrefab, transform.position, Quaternion.identity), 1);
        base.Despawned(runner, hasState);
    }
    #endregion
}