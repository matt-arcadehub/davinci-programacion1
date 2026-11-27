using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

public class MainMenuUI : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private Button newGameButton;

    [Header("Instructions Panel")]
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private Button instructionsButton;

    [Header("Credits Panel")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private Button creditsButton;

    [Header("Footer Hints")]
    [SerializeField] private GameObject primaryHintGroup;
    [SerializeField] private TMP_Text secondaryHintsText;
    [SerializeField] private Button backHintButton;

    private GameObject lastSelectedButton;
    private bool isLeavingScene;

    private enum HintType
    {
        Select,
        Back
    }

    private void Awake()
    {
        if (mainButtons != null)
        {
            mainButtons.SetActive(true);
            SetActiveHint(HintType.Select);
        }

        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }

        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }

    private void Start()
    {
        SelectButton(newGameButton);
    }

    private void Update()
    {
        if (EventSystem.current == null || mainButtons == null || !mainButtons.activeInHierarchy) return;

        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;

        if (selectedButton == lastSelectedButton) return;

        lastSelectedButton = selectedButton;

        if (selectedButton == null) return;

        if (selectedButton.transform.IsChildOf(mainButtons.transform))
        {
            GameSfx.Instance?.PlayUINavigate();
        }

        lastSelectedButton = selectedButton;
    }

    public void StartNewGame()
    {
        if (isLeavingScene) return;

        StartCoroutine(LoadSceneWithSound("Island_01"));
    }

    public void OpenPrototype()
    {
        if (isLeavingScene) return;

        StartCoroutine(LoadSceneWithSound("Prototype_TP1"));
    }

    private IEnumerator LoadSceneWithSound(string sceneName)
    {
        isLeavingScene = true;

        GameSfx.Instance?.PlayUIConfirm();

        float duration = GameSfx.Instance != null ? GameSfx.Instance.UiConfirmDuration : 0f;

        if (duration > 0f)
        {
            yield return new WaitForSecondsRealtime(duration);
        }

        SceneManager.LoadScene(sceneName);
    }

    public void ShowInstructions()
    {
        GameSfx.Instance?.PlayUIConfirm();

        mainButtons.SetActive(false);
        instructionsPanel.SetActive(true);

        SetActiveHint(HintType.Back);
        SelectButton(backHintButton);
    }

    public void CloseInstructions()
    {
        instructionsPanel.SetActive(false);
        mainButtons.SetActive(true);

        SetActiveHint(HintType.Select);
        SelectButton(instructionsButton);
    }

    public void ShowCredits()
    {
        GameSfx.Instance?.PlayUIConfirm();

        mainButtons.SetActive(false);
        creditsPanel.SetActive(true);

        SetActiveHint(HintType.Back);
        SelectButton(backHintButton);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        mainButtons.SetActive(true);

        SetActiveHint(HintType.Select);
        SelectButton(creditsButton);
    }

    public void QuitGame()
    {
        if (isLeavingScene) return;

        StartCoroutine(QuitWithSound());
    }

    private IEnumerator QuitWithSound()
    {
        isLeavingScene = true;

        GameSfx.Instance?.PlayUIBack();

        float duration = GameSfx.Instance != null ? GameSfx.Instance.UiBackDuration : 0f;

        if (duration > 0f)
        {
            yield return new WaitForSecondsRealtime(duration);
        }

        #if UNITY_EDITOR
            Debug.Log("Salir: disponible en la build.");
            isLeavingScene = false;
        #else
            Application.Quit();
        #endif
    }

    private void SelectButton(Button button)
    {
        if (button == null || EventSystem.current == null) return;

        EventSystem.current.SetSelectedGameObject(button.gameObject);
        lastSelectedButton = button.gameObject;
    }
    private void SetActiveHint(HintType hint)
    {
        bool isMainMenu = hint == HintType.Select;

        if (primaryHintGroup != null)
        {
            primaryHintGroup.SetActive(isMainMenu);
        }

        if (secondaryHintsText != null)
        {
            secondaryHintsText.text = isMainMenu ? "Salir" : "Volver";
        }

        if (backHintButton != null)
        {
            backHintButton.interactable = !isMainMenu;
        }
    }

    public void GoBack()
    {
        if (!instructionsPanel.activeSelf && !creditsPanel.activeSelf) return;

        GameSfx.Instance?.PlayUIBack();

        if (instructionsPanel.activeSelf)
        {
            CloseInstructions();
        }
        else if (creditsPanel.activeSelf)
        {
            CloseCredits();
        }
    }
}
