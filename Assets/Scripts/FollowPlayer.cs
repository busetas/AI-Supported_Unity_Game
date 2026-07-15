using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -7);
    public float smoothSpeed = 10f;
    public float collisionOffset = 0.3f;

    void LateUpdate()
    {
        // Kameranýn olmasý gereken ideal pozisyon
        Vector3 desiredPosition = target.TransformPoint(offset);

        // Raycast için yön ve mesafe
        Vector3 direction = desiredPosition - target.position;
        float distance = direction.magnitude;

        RaycastHit hit;

        // Karakter kamera arasý kontrol
        if (Physics.Raycast(target.position, direction.normalized, out hit, distance))
        {
            // Duvara çarptýysa kamerayý biraz öne al
            desiredPosition = hit.point - direction.normalized * collisionOffset;
        }

        // Yumuþak geçiþ
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        // Kamerayý karaktere baktýr
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
