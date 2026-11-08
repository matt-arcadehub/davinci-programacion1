using UnityEngine;

public class Spike : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float damage = 1f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Player>(out Player player)) return;

        PlayerFlight playerFlight = player.GetComponent<PlayerFlight>();

        if (playerFlight != null && !playerFlight.IsFlying) return;

        GameDebugLogger.Log(gameObject.name, $"colisionó con <b>{collision.gameObject.name}</b>");

        player.TakeDamage(damage);
        Destroy(gameObject);
    }
}
