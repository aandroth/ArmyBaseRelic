using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Camera_Behavior : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform m_targetTransform;

    [Header("Rubber Band Settings")]
    public float followSpeed = 5f;   // Higher = faster catch-up
    public float damping = 0.9f;     // Damping factor to reduce overshoot

    private float m_width, m_height;
    public Bounds m_cameraBounds;

    private Vector2 velocity;
    public float m_forwardOffset = 7f;
    public float m_verticalOffset = 1f;

    public Blackout m_blackout = null;


    private void Start()
    {
        m_height = Camera.main.orthographicSize;
        m_width = m_height * Camera.main.aspect;
        m_blackout.StartFadeOut();
    }

    public void SetMinAndMaxToWorldBounds()
    {
        m_height = Camera.main.orthographicSize;
        m_width = m_height * Camera.main.aspect;
        var minX = Globals.m_worldBounds.min.x + m_width;
        var maxX = Globals.m_worldBounds.max.x - m_width;

        var minY = Globals.m_worldBounds.min.y + m_height;
        var maxY = Globals.m_worldBounds.extents.y - m_height;

        m_cameraBounds = new Bounds();
        m_cameraBounds.SetMinMax(
            new Vector3(minX, minY, 0),
            new Vector3(maxX, maxY, 0)
            );
    }

    void FixedUpdate()
    {
        if (m_targetTransform == null) return;

        // Calculate direction to the target
        Vector2 direction = (Vector2)(m_targetTransform.position - transform.position);

        // Apply spring force (rubber band)
        velocity += direction * followSpeed * Time.fixedDeltaTime;

        // Apply damping to reduce oscillation
        velocity *= damping;

        // Move the follower
        Vector3 newPosition = transform.position;
        newPosition += (Vector3)(velocity * Time.fixedDeltaTime);
        newPosition.x += m_forwardOffset * Mathf.Sign(m_targetTransform.localScale.x);
        //newPosition.y += m_verticalOffset;
        newPosition = ApplyCameraBounds(newPosition);


        transform.position = newPosition;
    }

    private Vector3 ApplyCameraBounds(Vector3 newPosition)
    {
        return new Vector3(
            Mathf.Clamp(newPosition.x, m_cameraBounds.min.x, m_cameraBounds.max.x),
            Mathf.Clamp(newPosition.y, m_cameraBounds.min.y, m_cameraBounds.max.y),
            transform.position.z
            );

    }

    public void TriggerFadeIn()
    {
        m_blackout.StartFadeIn();
    }
    public void TriggerFadeOut()
    {
        m_blackout.StartFadeOut();
    }
}
