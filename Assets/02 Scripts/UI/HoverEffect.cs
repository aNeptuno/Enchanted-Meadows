using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Color hoverColor = new Color (0,1,0,0.5f);

    public Color hoverColorRed = new Color (1,0,0,1f);
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
        descriptionUI.selectedCrop = this.GetComponent<ChestItemUIController>().thisCrop;

        if (this.GetComponent<ChestItemUIController>().isStore)
        {
            if (descriptionUI.selectedCrop.seedCost > GameManager.Instance.playerCoins)
            {
                image.color = hoverColorRed;
            }
            else image.color = hoverColor;
        }
        else image.color = hoverColor;

        descriptionUI.ShowDescription();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = originalColor;
    }
}
