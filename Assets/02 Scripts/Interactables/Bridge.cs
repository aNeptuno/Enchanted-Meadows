using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bridge : MonoBehaviour
{
    #region "Singleton"
    public static Bridge Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // If not, set it to this instance
            //DontDestroyOnLoad(gameObject); // Make this instance persist across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy this instance if another instance already exists
        }
    }
    #endregion
    [SerializeField] private bool fromHouse;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Scene01")
        {
            fromHouse = true;
        }
        else if (SceneManager.GetActiveScene().name == "Scene02")
        {
            fromHouse = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("<color=yellow> Enter Trigger Soil</color>");
        if(other.CompareTag("Player"))
        {
            CanvasController.Instance.ShowBridgeUI(fromHouse);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.Instance.HideBridgeUI(fromHouse);
        }
    }

    public void Travel()
    {
        if (fromHouse)
        {
            MyDebugLog.Instance.MyDebugFunc("Travel to town");
            LoadTheScene("Scene02");

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayMusic("Town");
        }
        else
        {
            MyDebugLog.Instance.MyDebugFunc("Return home");
            LoadTheScene("Scene01");

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayMusic("MainTheme");
        }
    }

    void LoadTheScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        CanvasController.Instance.HideBridgeUI(!fromHouse);
    }


}
