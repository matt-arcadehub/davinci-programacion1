
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button understoodButton;

    private PlayerInteractor activeInteractor;
    private Action onConfirmed;

    public bool IsOpen => activeInteractor != null;

    public void Show(PlayerInteractor interactor, string message, Action confirmation)
    {
        if (IsOpen || interactor == null) return;

        if (dialoguePanel == null || dialogueText == null || understoodButton == null)
        {
            Debug.LogError("DialogueUI: faltan referencias.");

            return;
        }

        activeInteractor = interactor;
        onConfirmed = confirmation;
        dialogueText.text = message;
        understoodButton.interactable = false;

        dialoguePanel.SetActive(true);
        interactor.SetDialogueOpen(true);

        StartCoroutine(FocusButton());
    }

    private IEnumerator FocusButton()
    {
        yield return null;

        if (!IsOpen)
        {
            yield break;
        }

        understoodButton.interactable = true;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(understoodButton.gameObject);
        }
    }

    public void Confirm()
    {
        if (!IsOpen || !understoodButton.interactable) return;

        Action confirmation = onConfirmed;

        Close();
        confirmation?.Invoke();
    }

    public void Close()
    {
        if (!IsOpen) return;

        dialoguePanel.SetActive(false);

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        activeInteractor.SetDialogueOpen(false);

        activeInteractor = null;
        onConfirmed = null;
    }
}
