using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    #region "Singleton"
    public static SceneLoadManager Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // If not, set it to this instance
            DontDestroyOnLoad(gameObject); // Make this instance persist across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy this instance if another instance already exists
        }
    }
    #endregion
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("<color=yellow> Scene loaded (Scene load manager): " + scene.name+"</color>");
        if (scene.name == "Scene01")
        {
            if (GameManager.Instance != null)
                GameManager.Instance.GameInitialization();
            else Debug.Log("GameManager is null");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

