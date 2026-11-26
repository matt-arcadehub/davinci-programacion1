using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameSfx : MonoBehaviour
{
    [Header("UI Sounds")]
    [SerializeField] private AudioClip uiNavigateClip;
    [SerializeField] private AudioClip uiConfirmClip;
    [SerializeField] private AudioClip uiBackClip;
    public float UiConfirmDuration => uiConfirmClip != null ? uiConfirmClip.length : 0f;
    public float UiBackDuration => uiBackClip != null ? uiBackClip.length : 0f;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip brazierLitClip;

    [Header("Enemy Sounds")]
    [SerializeField] private AudioClip enemyAttackClip;
    [SerializeField] private AudioClip enemyHitClip;
    [SerializeField] private AudioClip enemyDeathClip;

    [Header("Pickup Sounds")]
    [SerializeField] private AudioClip energyPickupClip;
    [SerializeField] private AudioClip featherPickupClip;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)]
    private float sfxVolume = 0.7f;

    private AudioSource audioSource;

    public static GameSfx Instance { get; private set; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void PlayUINavigate()
    {
        PlayClip(uiNavigateClip);
    }

    public void PlayUIConfirm()
    {
        PlayClip(uiConfirmClip);
    }

    public void PlayUIBack()
    {
        PlayClip(uiBackClip);
    }

    public void PlayFire()
    {
        PlayClip(fireClip);
    }

    public void PlayDamage()
    {
        PlayClip(damageClip);
    }

    public void PlayBrazierLit()
    {
        PlayClip(brazierLitClip);
    }

    public void PlayEnemyAttack()
    {
        PlayClip(enemyAttackClip);
    }

    public void PlayEnemyHit()
    {
        PlayClip(enemyHitClip);
    }

    public void PlayEnemyDeath()
    {
        PlayClip(enemyDeathClip);
    }

    public void PlayEnergyPickup()
    {
        PlayClip(energyPickupClip);
    }

    public void PlayFeatherPickup()
    {
        PlayClip(featherPickupClip);
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.PlayOneShot(clip, sfxVolume);
    }
}
