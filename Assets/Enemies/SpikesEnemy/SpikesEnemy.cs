using System;
using UnityEngine;

public class SpikesEnemy : Enemy
{
    [Header("Attributes")]
    [SerializeField] private GameObject spikePrefab;
    [SerializeField, Range(4, 12)] private int spikes;
    [SerializeField] private float attackInterval = 3f;
    [SerializeField] private float attackRadius = 6f;

    private Transform playerTransform;
    private float attackTimer;

    private PlayerFlight playerFlight;

    protected override void Start()
    {
        base.Start();

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
            playerFlight = player.GetComponent<PlayerFlight>();
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        if (playerFlight != null && !playerFlight.IsFlying)
        {
            attackTimer = 0f;

            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRadius)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;
                Attack();
            }

        }
        else
        {
            attackTimer = 0f;
        }
    }

    void Attack()
    {
        if (spikePrefab == null)
        {
            Debug.LogWarning("No se puede realizar esta acción. No hay prefab para instanciar.");

            return;
        }
        else if (spikes <= 0)
        {
            Debug.LogWarning("No se puede realizar esta acción. No está definida la cantidad de pinchos.");

            return;
        }

        int angleBetweenSpikes = Mathf.RoundToInt(360f / spikes);

        for (int i = 0; i < spikes; i++)
        {
            float angle = i * angleBetweenSpikes;
            Quaternion rotationAngle = Quaternion.Euler(0f, 0f, angle);

            Instantiate(spikePrefab, transform.position, rotationAngle);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
