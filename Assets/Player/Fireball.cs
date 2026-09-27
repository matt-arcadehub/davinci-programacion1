using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 10f;
    public float damage = 1f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            GameDebugLogger.Log(gameObject.name, $"colisionó con <b>{collision.gameObject.name}</b>");
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
