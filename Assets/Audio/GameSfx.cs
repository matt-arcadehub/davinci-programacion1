using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameSfx : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip brazierLitClip;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)]
    private float sfxVolume = 0.7f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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

    private void PlayClip(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.PlayOneShot(clip, sfxVolume);
    }
}
