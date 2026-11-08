using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PursuerEnemy : Enemy
{
    [Header("Movement")]
    [SerializeField] private float detectionRadius = 4f;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float returnSpeed = 1.5f;
    [SerializeField] private float stopDistance = 0.35f;

    private Rigidbody2D rb;
    private Player player;
    private PlayerFlight playerFlight;
    private Vector2 homePosition;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        homePosition = rb.position;

        player = FindFirstObjectByType<Player>();

        if (player != null)
        {
            playerFlight = player.GetComponent<PlayerFlight>();
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition = homePosition;
        float speed = returnSpeed;

        if (player != null && playerFlight != null)
        {
            float distance = Vector2.Distance(rb.position, player.transform.position);

            if (playerFlight.IsFlying && distance <= detectionRadius)
            {
                targetPosition = player.transform.position;
                speed = chaseSpeed;
            }
        }

        if (Vector2.Distance(rb.position, targetPosition) <= stopDistance) return;

        Vector2 nextPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);

        rb.MovePosition(nextPosition);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
