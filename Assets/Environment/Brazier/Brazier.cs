using UnityEngine;

public class Brazier : MonoBehaviour
{
    [Header("Visual States")]
    [SerializeField] private GameObject unlitVisual;
    [SerializeField] private GameObject litVisual;

    public bool IsLit { get; private set; }

    private void Awake()
    {
        IsLit = false;

        UpdateVisuals();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsLit) return;

        Fireball fireball = other.GetComponentInParent<Fireball>();

        if (fireball == null) return;

        Light();
        Destroy(fireball.gameObject);
    }

    private void Light()
    {
        IsLit = true;

        UpdateVisuals();

        Debug.Log($"{gameObject.name} encendido.");
    }

    private void UpdateVisuals()
    {
        if (unlitVisual != null)
        {
            unlitVisual.SetActive(!IsLit);
        }

        if (litVisual != null)
        {
            litVisual.SetActive(IsLit);
        }
    }
}
