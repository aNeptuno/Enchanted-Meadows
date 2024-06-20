using UnityEngine;
using TMPro;

namespace Assets.SimpleLocalization.Scripts
{
    /// <summary>
    /// Localize TextMeshPro text component.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizeTextTMP : MonoBehaviour
    {
        public string LocalizationKey;

        private void Start()
        {
            Localize();
            LocalizationManager.OnLocalizationChanged += Localize;
        }

        private void OnDestroy()
        {
            LocalizationManager.OnLocalizationChanged -= Localize;
        }

        private void Localize()
        {
            GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize(LocalizationKey);
        }
    }
}
