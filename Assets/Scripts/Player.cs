using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D m_rigidbody;
    public float m_speed = 1.0f;

    public float m_speedCap = 10;
    public float m_stoppingFactor = 0.1f;
    public float m_stoppingThreshold = 0.1f;

    public Transform m_bulletSpawnPoint = null;
    public GameObject m_bulletPrefab = null;
    public GameObject m_gunArm = null;
    public Vector2 m_mousePosition = Vector2.zero;
    public enum PLAYER_ACTION_STATE {MOVING, STOPPING, STOPPED};
    public PLAYER_ACTION_STATE m_playerActionState = PLAYER_ACTION_STATE.STOPPED;

    Camera mainCamera;

    public Transform m_playerSpriteTransform;
    public SpriteRenderer m_playerSprite;
    public Sprite m_standingSprite;
    public Animator m_animator;
    public string m_runAnimationName;

    void Start()
    {
        // Cache the main camera for performance
        mainCamera = Camera.main;
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = false;
        if (Input.GetKey(KeyCode.D))
        {
            m_rigidbody.AddForce(m_speed * transform.right);
            isMoving = true;
            m_animator.enabled = true;
            m_animator?.Play(m_runAnimationName);
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_rigidbody.AddForce(-m_speed * transform.right);
            isMoving = true;
            m_animator.enabled = true;
            m_animator?.Play(m_runAnimationName);
        }

        if (isMoving)
        {
            m_playerActionState = PLAYER_ACTION_STATE.MOVING;

            if (isMoving && Mathf.Abs(m_rigidbody.linearVelocityX) > m_speedCap)
                m_rigidbody.linearVelocityX = m_speedCap * Mathf.Sign(m_rigidbody.linearVelocityX);
        }
        else if (m_playerActionState != PLAYER_ACTION_STATE.STOPPED)
        {
            if (!isMoving)
            {
                m_animator.enabled = false;
                m_playerSprite.sprite = m_standingSprite;

                if (Mathf.Abs(m_rigidbody.linearVelocityX) > m_stoppingThreshold)
                    m_rigidbody.linearVelocityX *= m_stoppingFactor;
                else
                {
                    m_rigidbody.linearVelocityX = 0;
                    m_playerActionState = PLAYER_ACTION_STATE.STOPPED;
                }
            }
        }


        // On left mouse click
        if (Input.GetMouseButtonDown(0) && m_bulletPrefab != null)
        {
            // Get the mouse position in screen space
            Vector3 mouseScreenPos = Input.mousePosition;

            // Convert mouse position to world space
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, mainCamera.WorldToScreenPoint(transform.position).z));

            // Calculate the direction vector from the GameObject to the mouse click
            Vector3 direction = mouseWorldPos - transform.position;

            GameObject bullet = GameObject.Instantiate(m_bulletPrefab, m_bulletSpawnPoint.transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet_Player>().Fire(direction);
        }


        {
            // Get the mouse position in screen space
            Vector3 mouseScreenPos = Input.mousePosition;

            // Convert mouse position to world space
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, mainCamera.WorldToScreenPoint(m_bulletSpawnPoint.position).z));
            mouseWorldPos.z = 0;

            m_gunArm.transform.right = Mathf.Sign(m_playerSprite.transform.localScale.x) * (mouseWorldPos - m_gunArm.transform.position);
            //Debug.Log($"mouseWorldPos: {mouseWorldPos.x}, {mouseWorldPos.y}, {mouseWorldPos.z} | m_gunArm: {m_gunArm.transform.position.x}, {m_gunArm.transform.position.y}, {m_gunArm.transform.position.z}");
            //Debug.DrawLine(transform.position, mouseWorldPos);

            if ((mouseWorldPos.x > transform.position.x && m_playerSprite.transform.localScale.x < 0) ||
                (mouseWorldPos.x < transform.position.x && m_playerSprite.transform.localScale.x > 0))
                m_playerSprite.transform.localScale = new Vector3(m_playerSprite.transform.localScale.x * -1,
                                                                  m_playerSprite.transform.localScale.y,
                                                                  m_playerSprite.transform.localScale.z);
        }
    }
}
