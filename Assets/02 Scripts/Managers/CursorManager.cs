using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    void Update()
    {
        // Check if the mouse is over any UI elements
        if (IsPointerOverUIElement())
        {
            // Show cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Hide cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private bool IsPointerOverUIElement()
    {
        // Return true if the pointer is over any UI element
        return EventSystem.current.IsPointerOverGameObject();
    }
}
