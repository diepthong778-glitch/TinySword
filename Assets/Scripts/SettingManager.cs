using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsOverlay : MonoBehaviour
{
    [SerializeField] private Canvas canvasRoot;
    [SerializeField] private GameObject panelRoot;

    private bool isOpen;

    void Awake()
    {
        // Force overlay behavior
        canvasRoot.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasRoot.sortingOrder = 1000;

        panelRoot.SetActive(false);
        isOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        panelRoot.SetActive(isOpen);
    }

    public void Open()
    {
        isOpen = true;
        panelRoot.SetActive(true);
    }

    public void Close()
    {
        isOpen = false;
        panelRoot.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
