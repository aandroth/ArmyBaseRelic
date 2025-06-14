using UnityEngine;

public class Bullet_Player : MonoBehaviour
{
    public float m_speed = 100f;
    public Vector2 m_direction;
    public float m_lifeTime = 5f;

    public void Fire(Vector2 direction)
    {
        transform.right = direction;
    }

    void Start()
    {
    }


    void Update()
    {
        transform.position += m_speed * transform.right * Time.deltaTime;
        m_lifeTime -= Time.deltaTime;
        if(m_lifeTime <= 0) Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}