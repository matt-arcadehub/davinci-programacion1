using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject interactionPrompt;

    public void SetPromptVisible(bool isVisible)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(isVisible);
        }
    }

    public abstract void Interact(PlayerInteractor interactor);

    public virtual void OnExitRange()
    {
    }
}