using UnityEngine;

public class FeatherOfLife : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.CollectFeatherOfLife();
            GameSfx.Instance?.PlayFeatherPickup();
            Destroy(gameObject);
        }
    }
}
