using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private Button defaultSelectedButton;

    [Header("Gameplay UI")]
    [SerializeField] private GameObject[] gameplayUIToHide;

    private void OnEnable()
    {
        HideGameplayUI();

        if (defaultSelectedButton == null || EventSystem.current == null) return;

        EventSystem.current.SetSelectedGameObject(defaultSelectedButton.gameObject);
    }

    private void HideGameplayUI()
    {
        foreach (GameObject element in gameplayUIToHide)
        {
            if (element == null) continue;

            element.SetActive(false);
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
