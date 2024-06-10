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

    public bool SetPlayerInBridge = false;
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("<color=yellow> Scene loaded (Scene load manager): " + scene.name+"</color>");
        if (scene.name == "Scene01")
        {
            if (GameManager.Instance != null && !SetPlayerInBridge)
                GameManager.Instance.GameInitialization(false);
            else if (GameManager.Instance != null && SetPlayerInBridge)
                GameManager.Instance.GameInitialization(true);

            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                if (SetPlayerInBridge)
                {
                    player.SetBridgePosition();
                    SetPlayerInBridge = false;
                }
                else player.SetInitialPosition();
            }

        }
        else if (scene.name == "Scene00")
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.StopEnvironment();
        }
        /* else if (scene.name == "Scene02")
        {
            if (GameManager.Instance != null && SetPlayerInBridge)
                GameManager.Instance.GameInitialization(true);
        } */
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

