using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private Button defaultSelectedButton;

    private void OnEnable()
    {
        if (defaultSelectedButton == null || EventSystem.current == null) return;

        EventSystem.current.SetSelectedGameObject(defaultSelectedButton.gameObject);
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
