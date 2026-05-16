using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LogoFade : LogoBase
{
    public float fadeTime = 1f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override IEnumerator PlayRoutine(Action onFinish)
    {
        Color c = image.color;

        // fade in
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            c.a = t / fadeTime;
            image.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        // fade out
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            c.a = 1 - (t / fadeTime);
            image.color = c;
            yield return null;
        }

        onFinish?.Invoke();
    }
}