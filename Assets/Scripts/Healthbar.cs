using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Healthbar : MonoBehaviour
{
    public Slider slider;
    public float shakeDuration = 0.15f;
    public float shakeStrength = 10f;

    private RectTransform rect;
    private Vector2 originalPos;
    private Coroutine shakeRoutine;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalPos = rect.anchoredPosition;
    }

    public void Shake()
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float timer = 0f;

        while (timer < shakeDuration)
        {
            Vector2 offset = Random.insideUnitCircle * shakeStrength;
            rect.anchoredPosition = originalPos + offset;

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        rect.anchoredPosition = originalPos;
    }
    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void setHealth(int health)
    {
        slider.value = health;
    }
}
