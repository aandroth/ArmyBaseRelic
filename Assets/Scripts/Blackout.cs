using System.Collections;
using UnityEngine;

public class Blackout : MonoBehaviour
{
    public float m_fadeOutTime = 3f;
    public float m_fadeInTime = 3f;
    public float m_fadeOutHoldTime = 0.5f;
    public float m_fadeInHoldTime = 0.5f;

    private float m_fadeOutTimeRemaining = 0f;
    private float m_fadeInTimePassed = 0f;
    public SpriteRenderer m_blackoutSprite;

    private IEnumerator m_fadeOutEnumerator;
    private IEnumerator m_fadeInEnumerator;

    public delegate void FadeOutDelegate();
    public FadeOutDelegate m_fadeOutDelegate;
    public delegate void FadeInDelegate();
    public FadeInDelegate m_fadeInDelegate;

    public void Start()
    {
        m_blackoutSprite = GetComponent<SpriteRenderer>();
    }

    public void StartFadeOut()
    {
        m_fadeOutEnumerator = FadeOutCoroutine();
        StartCoroutine(m_fadeOutEnumerator);
    }

    private IEnumerator FadeOutCoroutine()
    {
        Color color = m_blackoutSprite.color;
        while(color.a > 0)
        {
            m_fadeOutTimeRemaining -= Time.deltaTime;
            color.a = m_fadeOutTimeRemaining / m_fadeOutTime;
            m_blackoutSprite.color = color;
            yield return null;
        }
        m_fadeOutTimeRemaining = m_fadeOutTime;
        color.a = 0;
        m_blackoutSprite.color = color;

        float holdTime = m_fadeOutHoldTime;
        while (holdTime <= 0)
        {
            holdTime -= Time.deltaTime;
            yield return null;
        }
    }

    public void StartFadeIn()
    {
        m_fadeInEnumerator = FadeInCoroutine();
        StartCoroutine(m_fadeInEnumerator);
    }

    private IEnumerator FadeInCoroutine()
    {
        float holdTime = m_fadeInHoldTime;
        while (holdTime <= 0)
        {
            holdTime -= Time.deltaTime;
            yield return null;
        }

        Color color = m_blackoutSprite.color;
        while (color.a < 1)
        {
            m_fadeInTimePassed += Time.deltaTime;
            color.a = m_fadeInTimePassed / m_fadeInTime;
            m_blackoutSprite.color = color;
            yield return null;
        }
        m_fadeInTimePassed = m_fadeInTime;
        color.a = 1;
        m_blackoutSprite.color = color;
    }

    public void HardSetOpacity(float spriteOpacity)
    {
        StopCoroutine(m_fadeOutEnumerator);
        StopCoroutine(m_fadeInEnumerator);
        m_fadeOutTimeRemaining = m_fadeOutTime;
        m_fadeInTimePassed = 0;
        Color color = m_blackoutSprite.color;
        color.a = spriteOpacity;
        m_blackoutSprite.color = color;
    }
}
