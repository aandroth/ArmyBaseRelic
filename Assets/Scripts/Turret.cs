using UnityEngine;

public class Turret : MonoBehaviour
{
    public enum TURRET_STATE { UNPOWERED, SEARCHING, TARGET_ACQUIRED, FIRING, COOLDOWN}
    public TURRET_STATE m_turretState = TURRET_STATE.UNPOWERED;

    public int m_ammoCount = 100;
    public float m_cooldownTime = 1f;
    public float m_cooldownDecrement = 1f;

    public GameObject m_turretGun = null;
    public GameObject m_bullet = null;
    public GameObject m_bulletSpawnPoint = null;
    public GameObject m_target = null;
    public TargetDetector m_detector = null;

    public Sprite m_unpoweredSprite;
    public Animator m_animator = null;

    private void Start()
    {
        if(m_detector != null)
        {
            m_detector.m_targetAcquiredFunction = TargetAcquired;
            m_detector.m_targetLostFunction = TargetLost;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PowerOn();
        if (Input.GetKeyDown(KeyCode.U))
            PowerOff();


        switch (m_turretState)
        {
            case TURRET_STATE.UNPOWERED:
                break;
            case TURRET_STATE.SEARCHING:
                break;
            case TURRET_STATE.TARGET_ACQUIRED:
                m_turretState = TURRET_STATE.FIRING;
                m_turretGun.transform.right = m_target.transform.position - m_turretGun.transform.position;
                break;
            case TURRET_STATE.FIRING:
                m_turretGun.transform.right = m_target.transform.position - m_turretGun.transform.position;
                FireGun();
                break;
            case TURRET_STATE.COOLDOWN:
                CoolDown();
                m_turretGun.transform.right = m_target.transform.position - m_turretGun.transform.position;
                break;
            default:
                break;
        }
    }

    public void PowerOn()
    {
        m_detector.gameObject.SetActive(true);
        m_animator.enabled = true;
        m_turretState = TURRET_STATE.SEARCHING;
    }

    public void PowerOff()
    {
        m_detector.gameObject.SetActive(false);
        m_animator.enabled = false;
        m_turretGun.GetComponent<SpriteRenderer>().sprite = m_unpoweredSprite;
        m_turretState = TURRET_STATE.UNPOWERED;
    }
    

    public void TargetAcquired(GameObject gameObject)
    {
        m_turretState = TURRET_STATE.TARGET_ACQUIRED;
        m_target = gameObject;
        Debug.Log("Target acquired");
    }

    public void TargetLost()
    {
        m_turretState = TURRET_STATE.SEARCHING;
        m_target = null;
        Debug.Log("Target lost");
    }

    public void FireGun()
    {
        if(m_bullet != null && m_bulletSpawnPoint != null)
        {
            GameObject bulletInstance = GameObject.Instantiate(m_bullet, m_bulletSpawnPoint.transform.position, Quaternion.identity);
            bulletInstance.GetComponent<Bullet_Turret>().Fire(m_target.transform.position);
            m_cooldownDecrement = m_cooldownTime;
            m_turretState = TURRET_STATE.COOLDOWN;
        }
    }

    public void CoolDown()
    {
        m_cooldownDecrement -= Time.deltaTime;
        if(m_cooldownDecrement <= 0)
        {
            if (m_target != null)
                m_turretState = TURRET_STATE.FIRING;
            else
                m_turretState = TURRET_STATE.SEARCHING;
        }
    }
}
