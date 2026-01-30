using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    public Image fadeImage;
    public float fadeSpeed = 1.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(1f));
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(0f));
    }

    IEnumerator Fade(float targetAlpha)
    {
        Color c = fadeImage.color;
        while (!Mathf.Approximately(c.a, targetAlpha))
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
            fadeImage.color = c;
            yield return null;
        }
    }
}
