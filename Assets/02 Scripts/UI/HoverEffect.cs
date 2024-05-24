using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Color hoverColor = new Color (0,1,0,0.5f);
    private Color originalColor;
    private Image image;

    public ChestDescriptionUIController descriptionUI;

    void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = hoverColor;
        descriptionUI.selectedCrop = this.GetComponent<ChestItemUIController>().thisCrop;
        descriptionUI.ShowDescription();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = originalColor;
    }
}
