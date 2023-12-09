using Fusion;
using UnityEngine;

public class NegentraRocket : NetworkBehaviour
{
    // Public or inspector values
    [SerializeField] private GameObject rocketEffectPrefab;
    [SerializeField] private LayerMask playerMask;

    // Private values
    private Rigidbody2D myRigid;
    private TickTimer activitedTime = TickTimer.None;
    private bool activited = false;
    private bool notExploded = false;
    private Transform view;
    private int rocketRadius = 4;
    private Collider2D[] hitColliders = new Collider2D[10];
    private Vector2 inputDirection = Vector2.zero;

    #region Genel
    public void SetRocket(float timer, Vector2 input)
    {
        Vector2 direc = input;
        float angle = Mathf.Atan2(direc.y, direc.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rot;

        activitedTime = TickTimer.CreateFromSeconds(Runner, timer);
        inputDirection = input;
        inputDirection.Normalize();
    }
    private void ExplodeRocket()
    {
        notExploded = true;
        int hitsPlayerInRadius = Runner.GetPhysicsScene2D().OverlapCircle(view.position, rocketRadius, hitColliders, playerMask);

        for (int e = 0; e < hitsPlayerInRadius; e++)
        {
            if (hitColliders[e].TryGetComponent(out NegentraStats negentraStats))
            {
                negentraStats.UsedFoodEnergy(50);
            }
        }

        Runner.Despawn(Object);
        return;
    }
    #endregion

    #region Fusion
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        Destroy(Instantiate(rocketEffectPrefab, transform.position, Quaternion.identity), 3);
        base.Despawned(runner, hasState);
    }
    public override void Spawned()
    {
        base.Spawned();
        myRigid = GetComponent<Rigidbody2D>();
        view = transform.Find("View");
    }
    public override void FixedUpdateNetwork()
    {
        if (notExploded)
        {
            return;
        }
        if (Object.HasStateAuthority)
        {
            if (activited)
            {
                int hitsPlayer = Runner.GetPhysicsScene2D().OverlapCircle(view.position, 0.5f, hitColliders, playerMask);

                if (hitsPlayer > 0)
                {
                    ExplodeRocket();
                }
            }
            else
            {
                if (activitedTime.Expired(Runner))
                {
                    activitedTime = TickTimer.None;
                    activited = true;
                }
            }
            if (inputDirection.x < 0)
            {
                if (transform.position.x < Utils.MapSize * 0.5f * -1 + view.localScale.x * 0.5f)
                {
                    ExplodeRocket();
                }
            }
            else if (inputDirection.x > 0)
            {
                if (transform.position.x > Utils.MapSize * 0.5f - view.localScale.x * 0.5f)
                {
                    ExplodeRocket();
                }
            }

            if (inputDirection.y < 0)
            {
                if (transform.position.y < Utils.MapSize * 0.5f * -1 + view.localScale.y * 0.5f)
                {
                    ExplodeRocket();
                }
            }
            else if (inputDirection.y > 0)
            {
                if (transform.position.y > Utils.MapSize * 0.5f - view.localScale.y * 0.5f)
                {
                    ExplodeRocket();
                }
            }

            myRigid.AddForce(inputDirection * 5, ForceMode2D.Impulse);

            if (myRigid.velocity.magnitude > 5)
            {
                myRigid.velocity = myRigid.velocity.normalized * 5;
            }
        }
    }
    #endregion
}
