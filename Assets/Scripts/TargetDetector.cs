using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    public delegate void TargetAcquired(GameObject gameObject);
    public TargetAcquired m_targetAcquiredFunction;
    public delegate void TargetLost();
    public TargetLost m_targetLostFunction;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        m_targetAcquiredFunction(collision.gameObject);
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        m_targetLostFunction();
    }
}
