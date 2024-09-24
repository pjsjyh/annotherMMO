using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutController : MonoBehaviour
{
    public CanvasGroup canvasGroup; // Inspector에서 할당할 CanvasGroup

    // Fade Out (서서히 사라짐)
    public IEnumerator FadeOut(float duration)
    {
        float startAlpha = canvasGroup.alpha;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, t / duration);
            yield return null;
        }

        canvasGroup.alpha = 0; // 완전히 투명
    }

    // Fade In (서서히 나타남)
    public IEnumerator FadeIn(float duration)
    {
        float startAlpha = canvasGroup.alpha;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, t / duration);
            yield return null;
        }

        canvasGroup.alpha = 1; // 완전히 보임
    }
}
