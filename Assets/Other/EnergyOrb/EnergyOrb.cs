using UnityEngine;

public class EnergyOrb : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float energyToRecover = 1f;

    [Header("Movement Auto-collect")]
    [SerializeField] private float initialSpeed = 2f;
    [SerializeField] private float maxSpeed = 18f;
    [SerializeField] private float acceleration = 12f;

    [SerializeField] private Transform playerTransform;
    private float currentSpeed;

    private void Start()
    {
        currentSpeed = initialSpeed;
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        transform.position = Vector3.MoveTowards(
            transform.position,
            playerTransform.position,
            currentSpeed * Time.deltaTime
        );
    }

    public void SetEnergyToRecover(float energyToRecoverValue)
    {
        energyToRecover = energyToRecoverValue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.CollectEnergyOrb(energyToRecover);
            Destroy(gameObject);
        }
    }
}
