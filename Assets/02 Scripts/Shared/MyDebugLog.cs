using Unity.VisualScripting;
using UnityEngine;

public class MyDebugLog : MonoBehaviour
{
    #region "Singleton"
    public static MyDebugLog Instance { get; private set; } // Singleton instance

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

	public void MyDebugFunc(string msg, VariableDeclaration variable = null, string color = "yellow")
	{
		Debug.Log("<color="+color+">"+msg+ "" +variable+" </color>");
	}
}
