using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Visual States")]
    [SerializeField] private GameObject inactiveVisual;
    [SerializeField] private GameObject activeVisual;

    [Header("Victory UI")]
    [SerializeField] private GameObject victoryPanel;

    public bool IsActive { get; private set; }

    private bool victoryTriggered;

    private void Awake()
    {
        IsActive = false;
        victoryTriggered = false;

        UpdateVisuals();

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void Activate()
    {
        if (IsActive) return;

        IsActive = true;
        UpdateVisuals();

        Debug.Log("¡Portal activado!");
    }

    private void UpdateVisuals()
    {
        if (inactiveVisual != null)
        {
            inactiveVisual.SetActive(!IsActive);
        }

        if (activeVisual != null)
        {
            activeVisual.SetActive(IsActive);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsActive || victoryTriggered) return;

        Player player = other.GetComponentInParent<Player>();

        if (player == null) return;

        PlayerFlight flight = player.GetComponent<PlayerFlight>();

        if (flight == null || flight.IsFlying) return;

        TriggerVictory(player);
    }

    private void TriggerVictory(Player player)
    {
        victoryTriggered = true;

        player.SetControlsLocked(true);

        PlayerInteractor interactor = player.GetComponent<PlayerInteractor>();

        if (interactor != null)
        {
            interactor.enabled = false;
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }
}
