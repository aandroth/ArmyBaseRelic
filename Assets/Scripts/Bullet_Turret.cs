using UnityEngine;

public class Bullet_Turret : MonoBehaviour
{
    public float m_speed = 100f;
    public Vector3 m_direction;
    public float m_lifeTime = 5f;

    public void Fire(Vector3 targetPosition)
    {
        transform.right = targetPosition - transform.position;
    }

    void Update()
    {
        transform.position += m_speed * transform.right * Time.deltaTime;
        m_lifeTime -= Time.deltaTime;
        if (m_lifeTime <= 0) Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Turret hit {collision.gameObject.name}");
        Destroy(gameObject);
    }
}
