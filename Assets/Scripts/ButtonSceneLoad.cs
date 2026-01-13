using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoadButton : MonoBehaviour
{
    [Header("Level Settings")]
    [Tooltip("Leave empty to use build index")]
    public string sceneName;

    [Tooltip("Used if sceneName is empty")]
    public int sceneBuildIndex = -1;

    [Header("Lock Settings")]
    public bool checkUnlock = true;

    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadLevel);
    }

    void Start()
    {
        //if (checkUnlock)
        //    UpdateLockState();
    }

    void UpdateLockState()
    {
        //int targetIndex = GetTargetBuildIndex();

        //bool unlocked = targetIndex <= SaveManager.Instance.Data.highestLevelUnlocked + 1;
        //button.interactable = unlocked;
    }

    int GetTargetBuildIndex()
    {
        if (!string.IsNullOrEmpty(sceneName))
            return SceneUtility.GetBuildIndexByScenePath($"Assets/Scenes/{sceneName}.unity");

        return sceneBuildIndex;
    }

    public void LoadLevel()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else if (sceneBuildIndex >= 0)
        {
            SceneManager.LoadScene(sceneBuildIndex);
        }
        else
        {
            Debug.LogError("LevelLoadButton: No scene assigned!");
        }
    }
}
