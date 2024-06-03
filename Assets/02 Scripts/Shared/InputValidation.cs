using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputValidation : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        inputField = this.GetComponent<TMP_InputField>();
    }

    public void OnInputValueChanged(string newText)
    {
        // Limitar la longitud del texto a 20 caracteres
        if (newText.Length > 20)
        {
            inputField.text = newText.Substring(0, 20);
            return;
        }

        // Permitir solo caracteres alfanuméricos
        string filteredText = "";
        foreach (char c in newText)
        {
            if (char.IsLetterOrDigit(c))
            {
                filteredText += c;
            }
        }

        // Actualizar el texto del InputField con el texto filtrado
        inputField.text = filteredText;
    }
}
