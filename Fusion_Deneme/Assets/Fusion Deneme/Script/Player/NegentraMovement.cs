using Fusion;
using UnityEngine;

public class NegentraMovement : NetworkBehaviour
{
    // Private values
    private Rigidbody2D myRigid;
    private NegentraStats negentraStats;
    private CircleCollider2D myCollider2D;
    private Transform view;
    private Vector2 input = Vector2.zero;
    private Camera myCamera;
    private float movementSpeed = 1;
    private ushort playerSize = 1;
    
    // Properties
    public Vector2 Input { get { return input; } }

    #region Fusion
    public override void Spawned()
    {
        base.Spawned();
        myCamera = Camera.main;
        negentraStats = GetComponent<NegentraStats>();
        myRigid = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<CircleCollider2D>();
        view = transform.Find("View");

        input = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            input = networkInputData.inputMovement;
        }
        if (Object.HasStateAuthority)
        {
            Vector2 inputMovement = input;
            myRigid.velocity = Vector2.zero;

            if (inputMovement.x < 0)
            {
                if (transform.position.x < Utils.MapSize * 0.5f * -1 + view.localScale.x * 0.5f)
                {
                    inputMovement.x = 0;
                }
            }
            else if (inputMovement.x > 0)
            {
                if (transform.position.x > Utils.MapSize * 0.5f - view.localScale.x * 0.5f)
                {
                    inputMovement.x = 0;
                }
            }

            if (inputMovement.y < 0)
            {
                if (transform.position.y < Utils.MapSize * 0.5f * -1 + view.localScale.y * 0.5f)
                {
                    inputMovement.y = 0;
                }
            }
            else if (inputMovement.y > 0)
            {
                if (transform.position.y > Utils.MapSize * 0.5f - view.localScale.y * 0.5f)
                {
                    inputMovement.y = 0;
                }
            }
            inputMovement.Normalize();

            myRigid.AddForce(inputMovement * movementSpeed, ForceMode2D.Impulse);

            if (myRigid.velocity.magnitude > movementSpeed)
            {
                myRigid.velocity = myRigid.velocity.normalized * movementSpeed;
            }
        }
        ColliderControl();
    }
    #endregion

    #region Genel
    public void SetPlayerSize(ushort size)
    {
        playerSize = size;
        movementSpeed = size / Mathf.Pow(size, 1.1f) * 2;
    }
    public void StopRigidVelocity()
    {
        myRigid.velocity = Vector2.zero;
    }
    private void LateUpdate()
    {
        if (Object.HasInputAuthority)
        {
            myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, new Vector3(view.position.x, view.position.y, -15), Time.deltaTime);
        }
    }
    private void ColliderControl()
    {
        // Kendi colliderýmýzý kontrol etmeyelim diye false yapýyoruz
        myCollider2D.enabled = false;
        Collider2D hitCollider = Runner.GetPhysicsScene2D().OverlapCircle(view.position, (view.localScale.x * 0.5f) * 0.8f);
        myCollider2D.enabled = true;
        if (hitCollider != null)
        {
            if (hitCollider.CompareTag("Food"))
            {
                hitCollider.transform.position = Utils.LearnRandomSpawnPoint();
                negentraStats.CollectedFood(5);
            }
            if (hitCollider.CompareTag("Player"))
            {
                NegentraMovement otherMovement = hitCollider.GetComponent<NegentraMovement>();
                if (playerSize > otherMovement.playerSize)
                {
                    float playerHannibalFood = otherMovement.playerSize * 0.1f;
                    if (playerHannibalFood < 20)
                    {
                        playerHannibalFood = 20;
                    }

                    negentraStats.CollectedFood((ushort)playerHannibalFood);
                    negentraStats.IncreaseKill();

                    otherMovement.playerSize = 1;
                    hitCollider.GetComponent<NegentraPlayer>().PlayerDead();
                    hitCollider.GetComponent<NegentraStats>().IncreaseDead();
                }
            }
        }
    }
    #endregion
}