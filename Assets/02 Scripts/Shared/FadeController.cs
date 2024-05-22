
using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    public void ActivateWithFade()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        StartCoroutine(FadeIn());
    }

    public void DeactivateWithFade()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 0f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 1f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}


