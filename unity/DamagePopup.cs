using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private float time;
    private float moveSpeed = 2.0f;
    private float destroyTime = 1.0f;

    private void Update()
    {
        time += Time.deltaTime;

        // Calculate the interpolation factor based on time
        float t = Mathf.Clamp01(time / destroyTime);

        // Calculate the target position
        Vector3 targetPosition = transform.position + Vector3.up * 0.1f;

        // Lerp the position for smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, t);

        if (time > destroyTime)
        {
            Destroy(gameObject);
        }
    }
}