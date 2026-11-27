using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference pauseAction;

    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;

    [Header("Panels That Block Pausing")]
    [SerializeField] private GameObject[] blockingPanels;

    [Header("Player")]
    [SerializeField] private Player player;

    private bool isPaused;
    private GameObject lastSelectedButton;
    private bool isReturningToMenu;


    private void Awake()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.Disable();
        }

        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;

            player?.SetControlsLocked(false);
        }
    }

    private void Update()
    {
        if (isReturningToMenu) return;

        if (pauseAction != null && pauseAction.action.WasPressedThisFrame())
        {
            if (isPaused)
            {
                ResumeFromShortcut();
            }
            else
            {
                Pause();
            }
        }

        if (!isPaused || EventSystem.current == null) return;

        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;

        if (selectedButton == lastSelectedButton) return;

        lastSelectedButton = selectedButton;

        if (selectedButton != null && selectedButton.transform.IsChildOf(pausePanel.transform))
        {
            GameSfx.Instance?.PlayUINavigate();
        }
    }

    public void Pause()
    {
        if (isPaused || pausePanel == null) return;

        if (blockingPanels != null)
        {
            foreach (GameObject panel in blockingPanels)
            {
                if (panel != null && panel.activeInHierarchy) return;
            }
        }

        isPaused = true;
        Time.timeScale = 0f;

        pausePanel.SetActive(true);
        player?.SetControlsLocked(true);

        if (EventSystem.current != null && resumeButton != null)
        {
            EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
            lastSelectedButton = resumeButton.gameObject;

            GameSfx.Instance?.PlayUIConfirm();
        }
    }

    public void Resume()
    {
        ResumeGame(true);
    }

    private void ResumeFromShortcut()
    {
        ResumeGame(false);
    }

    private void ResumeGame(bool confirmed)
    {
        if (!isPaused || isReturningToMenu) return;

        if (confirmed)
        {
            GameSfx.Instance?.PlayUIConfirm();
        }
        else
        {
            GameSfx.Instance?.PlayUIBack();
        }

        isPaused = false;
        Time.timeScale = 1f;

        player?.SetControlsLocked(false);
        pausePanel.SetActive(false);
        lastSelectedButton = null;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void ReturnToMainMenu()
    {
        if (!isPaused || isReturningToMenu) return;

        StartCoroutine(ReturnToMainMenuWithSound());
    }

    private IEnumerator ReturnToMainMenuWithSound()
    {
        isReturningToMenu = true;

        GameSfx.Instance?.PlayUIConfirm();

        float duration = GameSfx.Instance != null ? GameSfx.Instance.UiConfirmDuration : 0f;

        if (duration > 0f)
        {
            yield return new WaitForSecondsRealtime(duration);
        }

        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene("MainMenu");
    }
}