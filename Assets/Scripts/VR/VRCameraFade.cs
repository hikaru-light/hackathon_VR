using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class VRCameraFade : MonoBehaviour
{
    [SerializeField]
    Image fadeImage;

    bool isFading = false;
    Action onFadeComplete = null;

    [SerializeField]
    Color fadeColor;


    public void FadeOut(float duration, Color endColor, Action onFadeComplete = null)
    {
        if (this.isFading) { return; }
        this.onFadeComplete = onFadeComplete;
        StartCoroutine(BeginFade(false, duration));
    }

    public void FadeIn(float duration, Color startColor, Action onFadeComplete = null)
    {
        if (this.isFading) { return; }
        this.onFadeComplete = onFadeComplete;
        StartCoroutine(BeginFade(true, duration));
    }

    IEnumerator BeginFade(bool isFadeIn, float duration)
    {
        this.isFading = true;
        float timer = 0f;
        float start = isFadeIn ? 1f : 0f;
        float end = isFadeIn ? 0f : 1f;
        while (timer <= duration)
        {
            this.fadeImage.color = new Color(
                this.fadeColor.r, this.fadeColor.b, this.fadeColor.g,
                Mathf.Lerp(start, end, timer / duration));
            timer += Time.deltaTime;
            yield return null;
        }
        this.isFading = false;
        if (this.onFadeComplete != null)
        {
            this.onFadeComplete();
        }
    }
}
