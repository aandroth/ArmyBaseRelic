using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Camera_Behavior : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform target;

    [Header("Rubber Band Settings")]
    public float followSpeed = 5f;   // Higher = faster catch-up
    public float damping = 0.9f;     // Damping factor to reduce overshoot

    private Vector2 velocity;

    void FixedUpdate()
    {
        if (target == null) return;

        // Calculate direction to the target
        Vector2 direction = (Vector2)(target.position - transform.position);

        // Apply spring force (rubber band)
        velocity += direction * followSpeed * Time.fixedDeltaTime;

        // Apply damping to reduce oscillation
        velocity *= damping;

        // Move the follower
        transform.position += (Vector3)(velocity * Time.fixedDeltaTime);
    }
}
