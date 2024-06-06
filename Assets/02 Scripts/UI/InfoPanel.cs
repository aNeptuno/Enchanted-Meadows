
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public GameObject infoPanel;

	public void OnPointerEnter(PointerEventData eventData)
    {
        // Show the info panel and update the text
        infoPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the info panel
        infoPanel.SetActive(false);
    }

}
